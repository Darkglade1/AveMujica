using AveMujica.AveMujicaCode.Extensions;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public sealed class AmorisAlly : AbstractAlly
{
  public static int StartingHP = 3;
  private static int damage = 3;
  private static int hits = 2;
  private static int strength = 1;
  private static int buffHits = 2;
  private static int autoSkillHPGain = 2;
  private static int skill1HPCost = 0;
  private static int skill2HPCost = 4;
  public override string CustomVisualPath => "amoris/amoris.tscn".CharacterPath();

  private int currentHits = hits;
  
  protected override MoveState GetDefaultMoveState()
  {
    return new MoveState("ATTACK_MOVE", Attack, new MultiAttackIntent(damage, currentHits));
  }
  
  private async Task Attack(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null && !ActedThisTurn)
    {
      await CreatureCmd.TriggerAnim(Creature, "Attack", 0f);
      IReadOnlyList<Creature>? hittableEnemies = Creature.CombatState?.HittableEnemies;
      if (hittableEnemies != null && hittableEnemies.Count != 0)
      {
        for (int i = 0; i < currentHits; i++)
        {
          foreach (var hittableEnemy in hittableEnemies)
          {
            await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), hittableEnemy, damage, ValueProp.Move, Creature);
          }
        }
      }
      await CreatureCmd.GainMaxHp(Creature, autoSkillHPGain);
    }
  }

  protected override void SetUpSkill1Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/AttackBuffIcon.png", 1);
  }

  protected override void SetUpSkill2Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/BuffIcon.png", 2);
  }
  
  public override async Task Skill1()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      ActedThisTurn = true;
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      await PaySkillCost(skill1HPCost);
      await PowerCmd.Apply<AsFierceAsFire>(new ThrowingPlayerChoiceContext(), owner.Creature, strength, Creature, null);
    }
  }
  
  public override async Task Skill2()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      ActedThisTurn = true;
      await CreatureCmd.TriggerAnim(Creature, "Cast2", 0);
      await PaySkillCost(skill2HPCost);
      currentHits += buffHits;
    }
  }
  
  public override HoverTip GetAutoSkillHoverTip()
  {
    return AutoSkillHoverTip();
  }

  public static HoverTip AutoSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_AUTO.title"),
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_AUTO.description"),
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_2.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, autoSkillHPGain, damage, hits);
    return hoverTip;
  }

  public override HoverTip GetSkill1HoverTip()
  {
    return Skill1HoverTip();
  }
  
  public static HoverTip Skill1HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill1HPCost, strength);
    return hoverTip;
  }

  public override HoverTip GetSkill2HoverTip()
  {
    return Skill2HoverTip();
  }
  
  public static HoverTip Skill2HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_2.title"),
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill2HPCost, buffHits);
    return hoverTip;
  }

  public static HoverTip GenerateCardHoverTip()
  {
    var startingHPText = GetStartingHPText(StartingHP);
    var autoSkillHoverTip = AutoSkillHoverTip();
    var skill1HoverTip = Skill1HoverTip();
    var skill2HoverTip = Skill2HoverTip();
    var hoverTipDescription = startingHPText + "\n" + autoSkillHoverTip.Description + "\n" + 
                              skill1HoverTip.Description + "\n" + skill2HoverTip.Description;
    return new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY.title"),
      hoverTipDescription);
  }

  public override int GetSkill1HPCost()
  {
    return skill1HPCost;
  }

  public override int GetSkill2HPCost()
  {
    return skill2HPCost;
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState startState = new AnimState("Start");
    AnimState animState = new AnimState("Idle", isLooping: true);
    AnimState animState2 = new AnimState("Attack");
    AnimState animState3 = new AnimState("Skill_1");
    AnimState animState4 = new AnimState("Skill_2");
    AnimState state = new AnimState("Die");
    startState.NextState = animState;
    animState2.NextState = animState;
    animState3.NextState = animState;
    animState4.NextState = animState;
    CreatureAnimator creatureAnimator = new CreatureAnimator(startState, controller);
    creatureAnimator.AddAnyState("Idle", animState);
    creatureAnimator.AddAnyState("Dead", state);
    creatureAnimator.AddAnyState("Attack", animState4);
    creatureAnimator.AddAnyState("Cast", animState2);
    creatureAnimator.AddAnyState("Cast2", animState3);
    return creatureAnimator;
  }
}

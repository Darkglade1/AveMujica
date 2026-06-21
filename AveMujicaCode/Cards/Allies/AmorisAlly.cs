using AveMujica.AveMujicaCode.Audio;
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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public sealed class AmorisAlly : AbstractAlly
{
  private static int damage = 1;
  private static int hits = 3;
  private static int strength = 1;
  private static int buffHits = 3;
  private static int skill1HPCost = 3;
  private static int skill2HPCost = 6;
  public override string CustomVisualPath => Config.UseAmorisSkin ? "amoris/skin/amoris.tscn".CharacterPath() : "amoris/amoris.tscn".CharacterPath();

  private int currentHits = hits;
  
  public override MoveState GetDefaultMoveState()
  {
    return new MoveState("ATTACK_MOVE", Attack, new MultiAttackIntent(damage, currentHits));
  }
  
  private async Task Attack(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsPerTurn = 0; // hack to prevent player from clicking skill button during enemy turn
      await CreatureCmd.TriggerAnim(Creature, "Attack", 0f);
      Sfx.SKILL_DRUM2.Play();
      for (int i = 0; i < currentHits; i++)
      {
        IReadOnlyList<Creature>? hittableEnemies = Creature.CombatState?.HittableEnemies;
        if (hittableEnemies != null)
        {
          var enemy = owner.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
          if (enemy != null)
          {
            await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), enemy, damage, ValueProp.Move, Creature);
          }
        }
      }
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
      numSkillsUsedThisTurn++;
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      Sfx.SKILL_DRUM.Play();
      await PaySkillCost(skill1HPCost);
      await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, strength, Creature, null);
    }
  }
  
  public override async Task Skill2()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsUsedThisTurn++;
      await CreatureCmd.TriggerAnim(Creature, "Cast2", 0);
      Sfx.SKILL_DRUM.Play();
      await PaySkillCost(skill2HPCost);
      currentHits += buffHits;
      SetMoveImmediate(GetDefaultMoveState());
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
    hoverTip.Description = String.Format(hoverTip.Description, damage, hits);
    return hoverTip;
  }
  
  public override HoverTip GetInCombatAutoSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_AUTO.title"),
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_AUTO.description"),
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_2.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, CalcAttackWithStr(damage), currentHits);
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
    var defaultText = new LocString("static_hover_tips", "AVEMUJICA-DEFAULT_TEXT.description");
    var autoSkillHoverTip = AutoSkillHoverTip();
    var skill1HoverTip = Skill1HoverTip();
    var skill2HoverTip = Skill2HoverTip();
    var hoverTipDescription = defaultText.GetFormattedText() + autoSkillHoverTip.Description + "\n" + 
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

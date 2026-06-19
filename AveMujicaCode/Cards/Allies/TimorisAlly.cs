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
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public sealed class TimorisAlly : AbstractAlly
{
  private static int damage = 4;
  private static int damageIncrease = 100;
  private static int debuff = 1;
  private static int skill1HPCost = 3;
  private static int skill2HPCost = 8;
  public override string CustomVisualPath => "timoris/timoris.tscn".CharacterPath();
  
  protected override MoveState GetDefaultMoveState()
  {
    return new MoveState("ATTACK_MOVE", Attack, new SingleAttackIntent(damage));
  }
  
  private async Task Attack(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsPerTurn = 0; // hack to prevent player from clicking skill button during enemy turn
      await CreatureCmd.TriggerAnim(Creature, "Attack", 0);
      Sfx.ATK_GUITAR.Play();
      IReadOnlyList<Creature>? hittableEnemies = Creature.CombatState?.HittableEnemies;
      if (hittableEnemies != null && hittableEnemies.Count != 0)
      {
        Creature? weakestEnemy = hittableEnemies.MinBy((Func<Creature, int>) (c => c.CurrentHp));
        if (weakestEnemy != null)
        {
          await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), weakestEnemy, damage, ValueProp.Move, Creature);
        }
      }
    }
  }

  protected override void SetUpSkill1Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/DebuffIcon.png", 1);
  }

  protected override void SetUpSkill2Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/StrongDebuffIcon.png", 2);
  }
  
  public override async Task Skill1()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsUsedThisTurn++;
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      Sfx.SKILL_BASS.Play();
      await PaySkillCost(skill1HPCost);
      if (Creature.CombatState != null)
      {
        foreach (Creature enemy in Creature.CombatState.HittableEnemies)
        {
          await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), enemy, debuff, Creature, null);
        }
      }
    }
  }
  
  public override async Task Skill2()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsUsedThisTurn++;
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      Sfx.SKILL_BASS.Play();
      await PaySkillCost(skill2HPCost);
      if (Creature.CombatState != null)
      {
        foreach (Creature enemy in Creature.CombatState.HittableEnemies)
        {
          await PowerCmd.Apply<MoonlightExecution>(new ThrowingPlayerChoiceContext(), enemy, damageIncrease, Creature, null);
        }
      }
    }
  }
  
  public override HoverTip GetAutoSkillHoverTip()
  {
    return AutoSkillHoverTip();
  }

  public static HoverTip AutoSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_AUTO.title"),
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_AUTO.description"),
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_2.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, damage);
    return hoverTip;
  }
  
  public override HoverTip GetInCombatAutoSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_AUTO.title"),
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_AUTO.description"),
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/attack/intent_attack_2.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, CalcAttackWithStr(damage));
    return hoverTip;
  }

  public override HoverTip GetSkill1HoverTip()
  {
    return Skill1HoverTip();
  }
  
  public static HoverTip Skill1HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill1HPCost, debuff);
    return hoverTip;
  }

  public override HoverTip GetSkill2HoverTip()
  {
    return Skill2HoverTip();
  }
  
  public static HoverTip Skill2HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_2.title"),
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill2HPCost, damageIncrease);
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
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY.title"),
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
    AnimState animState2 = new AnimState("Skill_1");
    AnimState state = new AnimState("Die");
    startState.NextState = animState;
    animState2.NextState = animState;
    CreatureAnimator creatureAnimator = new CreatureAnimator(startState, controller);
    creatureAnimator.AddAnyState("Idle", animState);
    creatureAnimator.AddAnyState("Dead", state);
    creatureAnimator.AddAnyState("Attack", animState2);
    creatureAnimator.AddAnyState("Cast", animState2);
    return creatureAnimator;
  }
}

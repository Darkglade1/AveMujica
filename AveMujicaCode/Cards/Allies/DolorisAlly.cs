using AveMujica.AveMujicaCode.Actions;
using AveMujica.AveMujicaCode.Extensions;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public sealed class DolorisAlly : AbstractAlly
{
  public static int StartingHP = 4;
  private static int block = 16;
  private static int damage = 6;
  private static int damageIncrease = 1;
  private static int playerStrength = 1;
  private static int autoSkillHPGain = 2;
  private static int skill1HPCost = 3;
  private static int skill2HPCost = 6;
  public override string CustomVisualPath => "doloris/doloris.tscn".CharacterPath();
  
  protected override MoveState GetDefaultMoveState()
  {
    return new MoveState("BUFF_MOVE", Buff, new BuffIntent());
  }
  
  private async Task Buff(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null && !ActedThisTurn)
    {
      ActedThisTurn = true;
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), owner.Creature, playerStrength, Creature, null);
      await CreatureCmd.GainMaxHp(Creature, autoSkillHPGain);
    }
  }

  protected override void SetUpSkill1Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/BlockIcon.png", 1);
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
      await CreatureCmd.GainBlock(owner.Creature, block, ValueProp.Unpowered, null);
    }
  }
  
  public override async Task Skill2()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      ActedThisTurn = true;
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      await PaySkillCost(skill2HPCost);
      await PowerCmd.Apply<HowDareYou>(new ThrowingPlayerChoiceContext(), owner.Creature, damage, Creature, null);
    }
  }
  
  public override HoverTip GetAutoSkillHoverTip()
  {
    return AutoSkillHoverTip();
  }

  public static HoverTip AutoSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_AUTO.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_AUTO.description"),
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_buff.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, autoSkillHPGain, playerStrength);
    return hoverTip;
  }

  public override HoverTip GetSkill1HoverTip()
  {
    return Skill1HoverTip();
  }
  
  public static HoverTip Skill1HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill1HPCost, block);
    return hoverTip;
  }

  public override HoverTip GetSkill2HoverTip()
  {
    return Skill2HoverTip();
  }
  
  public static HoverTip Skill2HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_2.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill2HPCost, damage, damageIncrease);
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
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY.title"),
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
    AnimState animState2 = new AnimState("Skill_1_Begin");
    AnimState animState3 = new AnimState("Skill_2_Begin");
    AnimState animState4 = new AnimState("Skill_2_Loop");
    AnimState animState5 = new AnimState("Skill_2_End");
    AnimState state = new AnimState("Die");
    startState.NextState = animState;
    animState2.NextState = animState;
    animState3.NextState = animState4;
    animState4.NextState = animState5;
    animState5.NextState = animState;
    CreatureAnimator creatureAnimator = new CreatureAnimator(startState, controller);
    creatureAnimator.AddAnyState("Idle", animState);
    creatureAnimator.AddAnyState("Dead", state);
    creatureAnimator.AddAnyState("Attack", animState4);
    creatureAnimator.AddAnyState("Cast", animState2);
    return creatureAnimator;
  }
}

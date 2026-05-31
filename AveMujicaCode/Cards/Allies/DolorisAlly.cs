using AveMujica.AveMujicaCode.Extensions;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public sealed class DolorisAlly : AbstractAlly
{
  private int block = 16;
  private int damage = 6;
  private int strength = 1;
  public int playerStrength = 1;
  private int autoSkillHPGain = 2;
  private int skill1HPCost = 3;
  private int skill2HPCost = 6;
  public override string CustomVisualPath => "doloris/doloris.tscn".CharacterPath();
  
  protected override MoveState GetDefaultMoveState()
  {
    return new MoveState("BUFF_MOVE", Buff, new DolorisBuffIntent(this));
  }
  
  private async Task Buff(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null && !ActedThisTurn)
    {
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
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      await CreatureCmd.GainBlock(owner.Creature, block, ValueProp.Unpowered, null);
      await PaySkillCost(skill1HPCost);
    }
  }
  
  public override async Task Skill2()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      ActedThisTurn = true;
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      await PowerCmd.Apply<HowDareYou>(new ThrowingPlayerChoiceContext(), owner.Creature, damage, Creature, null);
      await PaySkillCost(skill2HPCost);
    }
  }

  public override IHoverTip GetSkill1HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill1HPCost, block);
    return hoverTip;
  }

  public override IHoverTip GetSkill2HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_2.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill2HPCost, damage, strength);
    return hoverTip;
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

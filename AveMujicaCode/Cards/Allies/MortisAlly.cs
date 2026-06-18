using AveMujica.AveMujicaCode.Audio;
using AveMujica.AveMujicaCode.Extensions;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Audio;
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

public sealed class MortisAlly : AbstractAlly
{
  private static int block = 2;
  private static int cardDraw = 2;
  private static int intangible = 1;
  private static int skill1HPCost = 3;
  private static int skill2HPCost = 10;
  public override string CustomVisualPath => "mortis/mortis.tscn".CharacterPath();
  
  protected override MoveState GetDefaultMoveState()
  {
    return new MoveState("BLOCK_MOVE", Block, new DefendIntent());
  }
  
  private async Task Block(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null && canUseAbilitiesThisTurn)
    {
      numSkillsPerTurn = 0; // hack to prevent player from clicking skill button during enemy turn
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      await CreatureCmd.GainBlock(owner.Creature, CalcBlockWithDex(block), ValueProp.Unpowered, null);
    }
  }

  protected override void SetUpSkill1Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/BuffIcon.png", 1);
  }

  protected override void SetUpSkill2Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/BlockBuffIcon.png", 2);
  }
  
  public override async Task Skill1()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsUsedThisTurn++;
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      Sfx.SKILL_GUITAR.Play();
      await PaySkillCost(skill1HPCost);
      await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), cardDraw, owner);
    }
  }
  
  public override async Task Skill2()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsUsedThisTurn++;
      await CreatureCmd.TriggerAnim(Creature, "SwitchIn", 0);
      await PaySkillCost(skill2HPCost);
      await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(), Creature, intangible, Creature, null);
      await PowerCmd.Apply<DoNotFearDeath>(new ThrowingPlayerChoiceContext(), Creature, 1, Creature, null);
      canUseAbilitiesThisTurn = false;
      SetEmptyIntent();
    }
  }
  
  public override HoverTip GetAutoSkillHoverTip()
  {
    return AutoSkillHoverTip();
  }

  public static HoverTip AutoSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_AUTO.title"),
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_AUTO.description"),
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_defend.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, block);
    return hoverTip;
  }

  public override HoverTip GetSkill1HoverTip()
  {
    return Skill1HoverTip();
  }
  
  public static HoverTip Skill1HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill1HPCost, cardDraw);
    return hoverTip;
  }

  public override HoverTip GetSkill2HoverTip()
  {
    return Skill2HoverTip();
  }
  
  public static HoverTip Skill2HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_2.title"),
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill2HPCost, intangible);
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
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY.title"),
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
    AnimState animState2 = new AnimState("Skill_2");
    AnimState animState3 = new AnimState("Doll_Switchin");
    AnimState animState4 = new AnimState("Doll_Idle", isLooping: true);
    AnimState animState5 = new AnimState("Doll_SwitchOut");
    AnimState state = new AnimState("SwitchOut");
    startState.NextState = animState;
    animState2.NextState = animState;
    animState3.NextState = animState4;
    animState5.NextState = startState;
    CreatureAnimator creatureAnimator = new CreatureAnimator(startState, controller);
    creatureAnimator.AddAnyState("Idle", animState);
    creatureAnimator.AddAnyState("Dead", state);
    creatureAnimator.AddAnyState("Attack", animState2);
    creatureAnimator.AddAnyState("Cast", animState2);
    creatureAnimator.AddAnyState("SwitchIn", animState3);
    creatureAnimator.AddAnyState("SwitchOut", animState5);
    return creatureAnimator;
  }
}

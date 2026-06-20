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

public sealed class DolorisAlly : AbstractAlly
{
  private static int block = 2;
  private static int damage = 6;
  private static int damageIncrease = 1;
  private static int skillBlock = 7;
  private static int skill1HPCost = 3;
  private static int skill2HPCost = 8;
  public override string CustomVisualPath => Config.UseDolorisSkin ? "doloris/skin/doloris.tscn".CharacterPath() : "doloris/doloris.tscn".CharacterPath();
  
  protected override MoveState GetDefaultMoveState()
  {
    return new MoveState("BLOCK_MOVE", Block, new DefendIntent());
  }
  
  private async Task Block(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsPerTurn = 0; // hack to prevent player from clicking skill button during enemy turn
      await PlayCastAnimation();
      await CreatureCmd.GainBlock(owner.Creature, CalcBlockWithDex(block), ValueProp.Unpowered, null);
    }
  }

  protected override void SetUpSkill1Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/BlockIcon.png", 1);
  }

  protected override void SetUpSkill2Button()
  {
    SetUpSkillButton("res://AveMujica/images/charui/AttackBuffIcon.png", 2);
  }
  
  public override async Task Skill1()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsUsedThisTurn++;
      await PlayCastAnimation();
      Sfx.SKILL_GUITAR_VOCALS.Play();
      await PaySkillCost(skill1HPCost);
      await CreatureCmd.GainBlock(owner.Creature, CalcBlockWithDex(skillBlock), ValueProp.Unpowered, null);
    }
  }
  
  public override async Task Skill2()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      numSkillsUsedThisTurn++;
      await PlayCastAnimation();
      Sfx.SKILL_GUITAR_VOCALS.Play();
      await PaySkillCost(skill2HPCost);
      await PowerCmd.Apply<HowDareYou>(new ThrowingPlayerChoiceContext(), owner.Creature, damage, Creature, null);
    }
  }
  
  private async Task PlayCastAnimation()
  {
    if (Config.UseDolorisSkin)
    {
      await CreatureCmd.TriggerAnim(Creature, "Cast2", 0);
    }
    else
    {
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
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
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_defend.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, block);
    return hoverTip;
  }
  
  public override HoverTip GetInCombatAutoSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_AUTO.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_AUTO.description"),
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_defend.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, CalcBlockWithDex(block));
    return hoverTip;
  }

  public override HoverTip GetSkill1HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill1HPCost, CalcBlockWithDex(skillBlock));
    return hoverTip;
  }
  
  public static HoverTip Skill1HoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skill1HPCost, skillBlock);
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
    var defaultText = new LocString("static_hover_tips", "AVEMUJICA-DEFAULT_TEXT.description");
    var autoSkillHoverTip = AutoSkillHoverTip();
    var skill1HoverTip = Skill1HoverTip();
    var skill2HoverTip = Skill2HoverTip();
    var hoverTipDescription = defaultText.GetFormattedText() + autoSkillHoverTip.Description + "\n" + 
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
    AnimState animState6 = new AnimState("Skill_1_End");
    AnimState animState3 = new AnimState("Skill_2_Begin");
    AnimState animState4 = new AnimState("Skill_2_Loop");
    AnimState animState5 = new AnimState("Skill_2_End");
    AnimState state = new AnimState("Die");
    startState.NextState = animState;
    animState2.NextState = animState;
    animState3.NextState = animState4;
    animState4.NextState = animState5;
    animState5.NextState = animState;
    animState6.NextState = animState;
    CreatureAnimator creatureAnimator = new CreatureAnimator(startState, controller);
    creatureAnimator.AddAnyState("Idle", animState);
    creatureAnimator.AddAnyState("Dead", state);
    creatureAnimator.AddAnyState("Attack", animState4);
    creatureAnimator.AddAnyState("Cast", animState2);
    creatureAnimator.AddAnyState("Cast2", animState6);
    return creatureAnimator;
  }
}

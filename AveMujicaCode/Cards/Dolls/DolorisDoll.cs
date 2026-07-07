using AveMujica.AveMujicaCode.Audio;
using AveMujica.AveMujicaCode.Extensions;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Dolls;

public sealed class DolorisDoll : AbstractDoll
{
  private static int block = 3;
  private static int skillBlock = 6;
  public override string CustomVisualPath => Config.UseDolorisSkin ? "doloris/skin/doloris.tscn".CharacterPath() : "doloris/doloris.tscn".CharacterPath();
  
  public override MoveState GetDefaultMoveState()
  {
    return new MoveState("BLOCK_MOVE", Block, new DefendIntent());
  }
  
  private async Task Block(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await PlayCastAnimation();
      await CreatureCmd.GainBlock(owner.Creature, CalcBlockWithDex(block), ValueProp.Unpowered, null);
    }
  }
  
  public override async Task Skill()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await PlayCastAnimation();
      Sfx.SKILL_GUITAR_VOCALS.Play();
      await CreatureCmd.GainBlock(owner.Creature, CalcBlockWithDex(skillBlock), ValueProp.Unpowered, null);
    }
  }
  
  // public override async Task Skill2()
  // {
  //   var owner = Creature.PetOwner;
  //   if (owner != null)
  //   {
  //     numSkillsUsedThisTurn++;
  //     await PlayCastAnimation();
  //     Sfx.SKILL_GUITAR_VOCALS.Play();
  //     await PaySkillCost(skill2HPCost);
  //     await PowerCmd.Apply<HowDareYou>(new ThrowingPlayerChoiceContext(), owner.Creature, damage, Creature, null);
  //   }
  // }
  
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

  public override HoverTip GetSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, CalcBlockWithDex(skillBlock));
    return hoverTip;
  }
  
  public static HoverTip SkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skillBlock);
    return hoverTip;
  }

  public static HoverTip GenerateCardHoverTip()
  {
    var defaultText = new LocString("static_hover_tips", "AVEMUJICA-DEFAULT_TEXT.description");
    var autoSkillHoverTip = AutoSkillHoverTip();
    var skillHoverTip = SkillHoverTip();
    var hoverTipDescription = defaultText.GetFormattedText() + autoSkillHoverTip.Description + "\n" + 
                              skillHoverTip.Description;
    return new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY.title"),
      hoverTipDescription);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState startState = new AnimState("Start");
    AnimState animState = new AnimState("Idle", isLooping: true);
    AnimState animState2 = new AnimState("Skill_1_Begin");
    AnimState animState6 = new AnimState("Skill_1_End");
    AnimState animState3 = new AnimState("Skill_2_Begin");
    AnimState animState4 = new AnimState("Skill_2_Loop", isLooping: true);
    AnimState animState5 = new AnimState("Skill_2_End");
    AnimState state = new AnimState("Die");
    startState.NextState = animState;
    animState2.NextState = animState;
    animState3.NextState = animState4;
    animState5.NextState = animState;
    animState6.NextState = animState;
    CreatureAnimator creatureAnimator = new CreatureAnimator(startState, controller);
    creatureAnimator.AddAnyState("Idle", animState);
    creatureAnimator.AddAnyState("Dead", state);
    creatureAnimator.AddAnyState("Attack", animState3);
    creatureAnimator.AddAnyState("AttackEnd", animState5);
    creatureAnimator.AddAnyState("Cast", animState2);
    creatureAnimator.AddAnyState("Cast2", animState6);
    return creatureAnimator;
  }
}

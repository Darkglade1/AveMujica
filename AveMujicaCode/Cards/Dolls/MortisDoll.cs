using AveMujica.AveMujicaCode.Audio;
using AveMujica.AveMujicaCode.Extensions;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Dolls;

public sealed class MortisDoll : AbstractDoll
{
  private static int block = 3;
  private static int cardDraw = 2;
 
  public override string CustomVisualPath => Config.UseMortisSkin ? "mortis/skin/mortis.tscn".CharacterPath() : "mortis/mortis.tscn".CharacterPath();
  
  public override MoveState GetDefaultMoveState()
  {
    return new MoveState("BLOCK_MOVE", Block, new DefendIntent());
  }
  
  private async Task Block(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      await CreatureCmd.GainBlock(owner.Creature, CalcBlockWithDex(block), ValueProp.Unpowered, null);
    }
  }
  
  public override async Task Skill()
  {
    if (!canUseAbilitiesThisTurn)
    {
      return;
    }
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      Sfx.SKILL_GUITAR.Play();
      await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), cardDraw, owner);
    }
  }
  
  public async Task DoNotFearDeath()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await CreatureCmd.TriggerAnim(Creature, "SwitchIn", 0);
      await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(), Creature, 1, Creature, null);
      await PowerCmd.Apply<DoNotFearDeathPower>(new ThrowingPlayerChoiceContext(), Creature, 1, Creature, null);
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
  
  public override HoverTip GetInCombatAutoSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_AUTO.title"),
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_AUTO.description"),
      PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/intent_atlas.sprites/intent_defend.tres")));
    hoverTip.Description = String.Format(hoverTip.Description, CalcBlockWithDex(block));
    return hoverTip;
  }

  public override HoverTip GetSkillHoverTip()
  {
    return SkillHoverTip();
  }
  
  public static HoverTip SkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, cardDraw);
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
      new LocString("static_hover_tips", "AVEMUJICA-MORTIS_ALLY.title"),
      hoverTipDescription);
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

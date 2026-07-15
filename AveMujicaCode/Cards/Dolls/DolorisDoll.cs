using AveMujica.AveMujicaCode.Audio;
using AveMujica.AveMujicaCode.Cards.Uncommon;
using AveMujica.AveMujicaCode.Extensions;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Dolls;

public sealed class DolorisDoll : AbstractDoll
{
  private static int block = 3;
  private static int skillBlock = 6;
  private static int enhancedSkillBlock = 8;
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
      await CheckGiveBlockToAllPlayers(CalcBlockWithDex(block));
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
      await CheckGiveBlockToAllPlayers(CalcBlockWithDex(skillBlock));
    }
  }
  
  public override async Task EnhancedSkill()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await PlayCastAnimation();
      Sfx.SKILL_GUITAR_VOCALS.Play();
      await CreatureCmd.GainBlock(owner.Creature, CalcBlockWithDex(enhancedSkillBlock), ValueProp.Unpowered, null);
      await CheckGiveBlockToAllPlayers(CalcBlockWithDex(skillBlock));
      await PowerCmd.Apply<BlurPower>(new ThrowingPlayerChoiceContext(), owner.Creature, 1, Creature, null);
    }
  }

  private async Task CheckGiveBlockToAllPlayers(int amount)
  {
    var owner = Creature.PetOwner;
    if (owner != null && owner.Creature.HasPower<HymnPower>())
    {
      foreach (Player player in CombatState.Players)
      {
        if (player != owner)
        {
          await CreatureCmd.GainBlock(player.Creature, amount, ValueProp.Unpowered, null);
        }
      }
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

  public override HoverTip GetSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-SKILL_TEXT.description"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, CalcBlockWithDex(skillBlock));
    return hoverTip;
  }
  
  public override HoverTip GetEnhancedSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-ENHANCED_SKILL_TEXT.description"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, CalcBlockWithDex(enhancedSkillBlock));
    return hoverTip;
  }
  
  public static HoverTip SkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-SKILL_TEXT.description"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, skillBlock);
    return hoverTip;
  }
  
  public static HoverTip EnhancedSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_2.title"),
      new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, enhancedSkillBlock);
    return hoverTip;
  }

  public static HoverTip GenerateCardHoverTip()
  {
    var defaultText = new LocString("static_hover_tips", "AVEMUJICA-DEFAULT_TEXT.description");
    var skillText = new LocString("static_hover_tips", "AVEMUJICA-SKILL_TEXT.description");
    var enhancedSkillText = new LocString("static_hover_tips", "AVEMUJICA-ENHANCED_SKILL_TEXT.description");
    var autoSkillHoverTip = AutoSkillHoverTip();
    var skillHoverTip = SkillHoverTip();
    var enhancedSkillHoverTip = EnhancedSkillHoverTip();
    var hoverTipDescription = defaultText.GetFormattedText() + autoSkillHoverTip.Description + "\n" + 
                              "[gold]" + skillText.GetFormattedText() + ":" + "[/gold] " + skillHoverTip.Description + "\n" +
                              "[gold]" + enhancedSkillText.GetFormattedText() + ":" + "[/gold] " + enhancedSkillHoverTip.Description;
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

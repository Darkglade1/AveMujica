using AveMujica.AveMujicaCode.Audio;
using AveMujica.AveMujicaCode.Extensions;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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

public sealed class AmorisDoll : AbstractDoll
{
  private static int damage = 2;
  public static int baseHits = 2;
  private static int strength = 1;
  public override string CustomVisualPath => Config.UseAmorisSkin ? "amoris/skin/amoris.tscn".CharacterPath() : "amoris/amoris.tscn".CharacterPath();

  public int currentHits = baseHits;
  
  public override MoveState GetDefaultMoveState()
  {
    return new MoveState("ATTACK_MOVE", Attack, new MultiAttackIntent(damage, currentHits));
  }
  
  private async Task Attack(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await CreatureCmd.TriggerAnim(Creature, "Attack", 0f);
      if (canPlaySFX)
      {
        canPlaySFX = false;
        Sfx.SKILL_DRUM.Play();
      }
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
  
  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner == Creature.PetOwner)
    {
      canPlaySFX = true;
    }
    return Task.CompletedTask;
  }
  
  public override async Task Skill()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await PowerCmd.Apply<AmorisTempStrPower>(new ThrowingPlayerChoiceContext(), Creature, strength, Creature, null);
      await Attack(null);
    }
  }
  
  public override async Task EnhancedSkill()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, strength, Creature, null);
      await Attack(null);
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
    hoverTip.Description = String.Format(hoverTip.Description, damage, baseHits);
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

  public override HoverTip GetSkillHoverTip()
  {
    return SkillHoverTip();
  }
  
  public override HoverTip GetEnhancedSkillHoverTip()
  {
    return EnhancedSkillHoverTip();
  }
  
  public static HoverTip SkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-SKILL_TEXT.description"),
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, strength);
    return hoverTip;
  }
  
  public static HoverTip EnhancedSkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-ENHANCED_SKILL_TEXT.description"),
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, strength);
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
      new LocString("static_hover_tips", "AVEMUJICA-AMORIS_ALLY.title"),
      hoverTipDescription);
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

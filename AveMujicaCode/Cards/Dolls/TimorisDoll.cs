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

public sealed class TimorisDoll : AbstractDoll
{
  private static int damage = 5;
  private static int debuff = 1;
  public override string CustomVisualPath => Config.UseTimorisSkin ? "timoris/skin/timoris.tscn".CharacterPath() : "timoris/timoris.tscn".CharacterPath();
  
  public override MoveState GetDefaultMoveState()
  {
    return new MoveState("ATTACK_MOVE", Attack, new SingleAttackIntent(damage));
  }
  
  private async Task Attack(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
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
  
  public override async Task Skill()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      if (UseMoonlightExecution())
      {
        await MoonlightExecution();
      }
      else
      {
        await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
        Sfx.SKILL_BASS.Play();
        IReadOnlyList<Creature>? hittableEnemies = Creature.CombatState?.HittableEnemies;
        if (hittableEnemies != null && hittableEnemies.Count != 0)
        {
          Creature? strongestEnemy = hittableEnemies.MaxBy((Func<Creature, int>) (c => c.CurrentHp));
          if (strongestEnemy != null)
          {
            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), strongestEnemy, debuff, Creature, null);
            await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), strongestEnemy, debuff, Creature, null);
          }
        }
      }
    }
  }
  
  public async Task MoonlightExecution()
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      Sfx.SKILL_BASS.Play();
      if (Creature.CombatState != null)
      {
        foreach (Creature enemy in Creature.CombatState.HittableEnemies)
        {
          await PowerCmd.Apply<MoonlightExecutionDebuff>(new ThrowingPlayerChoiceContext(), enemy, GetMoonlightExecutionAmount(), Creature, null);
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

  public override HoverTip GetSkillHoverTip()
  {
    if (UseMoonlightExecution())
    {
      return MoonlightExecutionHovertip();
    }
    return SkillHoverTip();
  }
  
  public static HoverTip SkillHoverTip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_1.title"),
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_1.description"));
    hoverTip.Description = String.Format(hoverTip.Description, debuff);
    return hoverTip;
  }
  
  public HoverTip MoonlightExecutionHovertip()
  {
    var hoverTip = new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_2.title"),
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY_SKILL_2.description"));
    hoverTip.Description = String.Format(hoverTip.Description, GetMoonlightExecutionAmount());
    return hoverTip;
  }

  public static HoverTip GenerateCardHoverTip()
  {
    var defaultText = new LocString("static_hover_tips", "AVEMUJICA-DEFAULT_TEXT.description");
    var autoSkillHoverTip = AutoSkillHoverTip();
    var skill1HoverTip = SkillHoverTip();
    var hoverTipDescription = defaultText.GetFormattedText() + autoSkillHoverTip.Description + "\n" + 
                              skill1HoverTip.Description;
    return new HoverTip(
      new LocString("static_hover_tips", "AVEMUJICA-TIMORIS_ALLY.title"),
      hoverTipDescription);
  }

  private bool UseMoonlightExecution()
  {
    if (Creature.PetOwner != null)
    {
      return Creature.PetOwner.Creature.HasPower<MoonlightExecutionPower>();
    }
    return false;
  }
  
  private int GetMoonlightExecutionAmount()
  {
    if (Creature.PetOwner != null && Creature.PetOwner.Creature.HasPower<MoonlightExecutionPower>())
    {
      return Creature.PetOwner.Creature.GetPowerAmount<MoonlightExecutionPower>();
    }
    return 0;
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

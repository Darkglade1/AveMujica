using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public abstract class AbstractAlly : CustomMonsterModel
{
  public AbstractAlly()
  {
    CustomContentDictionary.RegisterType(GetType());
  }
  public override int MinInitialHp => 1;

  public override int MaxInitialHp => 1;

  public override bool IsHealthBarVisible => Creature.IsAlive;

  public bool ActedThisTurn;
  private bool hasSetUp;

  protected abstract MoveState GetDefaultMoveState();

  public override Task AfterCurrentHpChanged(Creature creature, Decimal delta)
  {
    if (!hasSetUp)
    {
      SetMoveImmediate(GetDefaultMoveState());
      SetUpSkill1Button();
      SetUpSkill2Button();
      hasSetUp = true;
    }
    return Task.CompletedTask;
  }
  
  protected override MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    MoveState initialState = GetDefaultMoveState();
    initialState.FollowUpState = initialState;
    return new MonsterMoveStateMachine(
      new List<MonsterState> { initialState },
      initialState
    );
  }
  
  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (side == CombatSide.Player)
    {
      ActedThisTurn = false;
      SetMoveImmediate(GetDefaultMoveState());
    }
    return Task.CompletedTask;
  }
  
  public override async Task BeforeSideTurnEndEarly(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side == CombatSide.Player)
    {
      await PerformMove();
    }
  }

  protected void SetUpSkillButton(string path, int skillNum)
  {
    NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(Creature);
    Marker2D? specialNode = creatureNode?.GetSpecialNode<Marker2D>("%IntentPos");
    if (specialNode != null)
    {
      var buttonScene = GD.Load<PackedScene>("res://AveMujica/images/charui/ally_button.tscn");
      if (buttonScene != null)
      {
        var button = buttonScene.Instantiate<NAllyButton>();
        if (button != null)
        {
          TextureRect? textureNode = button.GetNodeOrNull<TextureRect>("%ButtonVisual");
          if (textureNode != null)
          {
            textureNode.Texture = GD.Load<Texture2D>(path);
          }
          button.owner = this;
          button.skillNum = skillNum;
          specialNode.AddChildSafely(button);
          if (skillNum == 1)
          {
            button.Position += new Vector2(-125f, 0f);
          }
          else
          {
            button.Position += new Vector2(-125f, 75f);
          }
        }
      }
    }
  }

  protected void SetEmptyIntent()
  {
    ActedThisTurn = true;
    MoveState emptyState = new MoveState("NOTHING_MOVE", _ => Task.CompletedTask);
    emptyState.FollowUpState = emptyState;
    SetMoveImmediate(emptyState);
  }

  protected async Task PaySkillCost(int skillCost)
  {
    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Creature, skillCost, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Creature);
    await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Creature, skillCost, false);
    SetEmptyIntent();
  }

  protected abstract void SetUpSkill1Button();
  
  protected abstract void SetUpSkill2Button();

  public abstract Task Skill1();
  
  public abstract Task Skill2();
  
  public abstract HoverTip GetAutoSkillHoverTip();
  
  public abstract HoverTip GetSkill1HoverTip();
  
  public abstract HoverTip GetSkill2HoverTip();

  public abstract int GetSkill1HPCost();

  public abstract int GetSkill2HPCost();

  public static string GetStartingHPText(int startingHP)
  {
    var startingHPLoc = new LocString("static_hover_tips", "AVEMUJICA-ALLY_STARTING_HP.description");
    var startingHPText = startingHPLoc.GetFormattedText();
    return String.Format(startingHPText, startingHP);
  }
}

[HarmonyPatch(typeof(AbstractIntent), nameof(AbstractIntent.GetHoverTip))]
public static class SetAllyIntentHoverTip
{
  public static void Postfix(AbstractIntent __instance, IEnumerable<Creature> targets, Creature owner, ref HoverTip __result)
  {
    if (owner.Monster is AbstractAlly ally)
    {
      __result = ally.GetAutoSkillHoverTip();
    }
  }
}

[HarmonyPatch(typeof(PersonalHivePower), nameof(PersonalHivePower.AfterDamageReceived))]
public static class PatchEntomancer
{
  public static bool Prefix(PersonalHivePower __instance, PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult _,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    ref Task __result)
  {
    if (dealer != null && dealer.Monster is AbstractAlly)
    {
      __result = Task.CompletedTask;
      return false;
    }

    return true;
  }
}

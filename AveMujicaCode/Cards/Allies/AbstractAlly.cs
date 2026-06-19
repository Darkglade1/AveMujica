using AveMujica.AveMujicaCode.Hooks;
using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
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
  public override int MinInitialHp => 1;

  public override int MaxInitialHp => 1;

  public override bool IsHealthBarVisible => Creature.IsAlive;

  public int baseNumSkillsPerTurn = 1;
  public int numSkillsPerTurn = 1;
  protected int numSkillsUsedThisTurn;
  protected bool canUseAbilitiesThisTurn = true;
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
  
  protected void SetEmptyIntent()
  {
    MoveState emptyState = new MoveState("NOTHING_MOVE", _ => Task.CompletedTask);
    emptyState.FollowUpState = emptyState;
    SetMoveImmediate(emptyState);
  }
  
  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (side == CombatSide.Player)
    {
      numSkillsPerTurn = baseNumSkillsPerTurn;
      numSkillsUsedThisTurn = 0;
      canUseAbilitiesThisTurn = true;
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
    if (!LocalContext.IsMe(Creature.PetOwner))
    {
      return;
    }
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

  public bool CanUseSkill()
  {
    return numSkillsPerTurn > numSkillsUsedThisTurn && canUseAbilitiesThisTurn;
  }

  protected async Task PaySkillCost(int skillCost)
  {
    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Creature, skillCost, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Creature);
    await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Creature, skillCost, false);
    if (Creature.PetOwner != null)
    {
      await AveMujicaHooks.AfterDollSkill(Creature.PetOwner.RunState, Creature.PetOwner.Creature.CombatState, Creature.PetOwner, this);
    }
  }

  protected abstract void SetUpSkill1Button();
  
  protected abstract void SetUpSkill2Button();

  public abstract Task Skill1();
  
  public abstract Task Skill2();
  
  public abstract HoverTip GetAutoSkillHoverTip();
  public abstract HoverTip GetInCombatAutoSkillHoverTip();
  
  public abstract HoverTip GetSkill1HoverTip();
  
  public abstract HoverTip GetSkill2HoverTip();

  public abstract int GetSkill1HPCost();

  public abstract int GetSkill2HPCost();

  protected int CalcBlockWithDex(int block)
  {
    int dexAmt = 0;
    var dexterity = Creature.GetPower<DexterityPower>();
    if (dexterity != null)
    {
      dexAmt += dexterity.Amount;
    }
    return block + dexAmt;
  }
  protected int CalcAttackWithStr(int damage)
  {
    int strAmt = 0;
    var strength = Creature.GetPower<StrengthPower>();
    if (strength != null)
    {
      strAmt += strength.Amount;
    }
    return damage + strAmt;
  }
}

using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Dolls;

public abstract class AbstractDoll : CustomMonsterModel
{
  public override int MinInitialHp => 3;

  public override int MaxInitialHp => 3;

  public override bool IsHealthBarVisible => Creature.IsAlive;
  
  protected bool canUseAbilitiesThisTurn = true;
  private bool hasSetUp;

  public abstract MoveState GetDefaultMoveState();

  public override Task AfterCurrentHpChanged(Creature creature, Decimal delta)
  {
    if (!hasSetUp)
    {
      SetMoveImmediate(GetDefaultMoveState());
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
      canUseAbilitiesThisTurn = true;
    }
    return Task.CompletedTask;
  }
  
  public override async Task BeforeSideTurnEndEarly(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side == CombatSide.Player && Creature.IsAlive && Creature.PetOwner != null && Creature.PetOwner.Creature.IsAlive && canUseAbilitiesThisTurn)
    {
      await PerformMove();
      await Cmd.Wait(0.25f);
      await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Creature, 1, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Creature);
      await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Creature, 1, false);
    }
  }
  
  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (creature == Creature.PetOwner?.Creature && !wasRemovalPrevented)
    { 
      await CreatureCmd.Kill(Creature);
    }
  }

  public abstract Task Skill();
  
  public abstract HoverTip GetAutoSkillHoverTip();
  public abstract HoverTip GetInCombatAutoSkillHoverTip();
  
  public abstract HoverTip GetSkillHoverTip();

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
  public override string DeathSfx => "death_operator.ogg".AudioPath();
}

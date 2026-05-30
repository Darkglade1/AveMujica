using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public abstract class AbstractAlly : CustomMonsterModel
{
  public override int MinInitialHp => 1;

  public override int MaxInitialHp => 1;

  public override bool IsHealthBarVisible => Creature.IsAlive;

  protected abstract MoveState GetDefaultMoveState();

  public override Task AfterCurrentHpChanged(Creature creature, Decimal delta)
  {
    if (NextMove != GetDefaultMoveState())
    {
      SetMoveImmediate(GetDefaultMoveState());
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
}

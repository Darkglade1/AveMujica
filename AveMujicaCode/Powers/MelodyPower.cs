using AveMujica.AveMujicaCode.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class MelodyPower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player && Owner.Player != null)
        {
            if (PerformedThisTurn)
            {
                Flash();
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, Owner, null);
            }
        }
    }
    
    public bool PerformedThisTurn
    {
        get
        {
            return CombatManager.Instance.History.Entries.OfType<PerformCardEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner.Player);
        }
    }
}
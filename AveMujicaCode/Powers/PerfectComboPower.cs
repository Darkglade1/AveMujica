using AveMujica.AveMujicaCode.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class PerfectComboPower : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (applier == Owner)
        {
            UpdateCounter();
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature == Owner)
        {
            UpdateCounter();
        }
        return Task.CompletedTask;
    }
    
    public void UpdateCounter()
    {
        var numTriggers = CombatManager.Instance.History.Entries.OfType<PerformCardEntry>()
            .Count(e => e.Card.Owner.Creature == Owner && e.HappenedThisTurn(CombatState));
        numTriggers = PerformCard.PerfectComboStormCap(numTriggers);
        Amount = numTriggers;
    }
}
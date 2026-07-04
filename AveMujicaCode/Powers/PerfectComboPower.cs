using AveMujica.AveMujicaCode.Cards;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class PerfectComboPower : AveMujicaPower, IHasSecondAmount
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new("StormCounter",0)];
    
    public override Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (applier == Owner && power is PerfectComboPower)
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
        DynamicVars["StormCounter"].BaseValue = AbstractPerformCard.PerfectComboStormCap(numTriggers);
        this.InvokeSecondAmountChanged();
    }

    public string GetSecondAmount()
    {
        return DynamicVars["StormCounter"].IntValue.ToString();
    }
}
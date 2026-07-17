using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Hooks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace AveMujica.AveMujicaCode.Relics;

public class MasqueradeMask() : AveMujicaRelic, IAfterPerform
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform)];
    
    private bool _usedThisTurn;
    
    public bool UsedThisTurn
    {
        get => _usedThisTurn;
        set
        {
            AssertMutable();
            _usedThisTurn = value;
        }
    }
    
    public async Task AfterPerform(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Card.Owner == Owner && !UsedThisTurn)
        {
            Flash();
            UsedThisTurn = true;
            await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Cards.IntValue, Owner.Creature, null);
        }
    }
    
    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
        {
            return Task.CompletedTask;
        }
        UsedThisTurn = false;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        UsedThisTurn = false;
        return Task.CompletedTask;
    }
}
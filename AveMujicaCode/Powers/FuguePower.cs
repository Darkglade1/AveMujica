using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class FuguePower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombatLate(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != Owner || (!card.Keywords.Contains(CardKeyword.Retain) && !card.Keywords.Contains(CardKeyword.Exhaust)))
            return false;
        PileType? type = card.Pile?.Type;
        if (type == PileType.Hand || type == PileType.Play)
        {
            modifiedCost = 0;
            return true;
        }
        return false;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner || (!cardPlay.Card.Keywords.Contains(CardKeyword.Retain) && !cardPlay.Card.Keywords.Contains(CardKeyword.Exhaust)))
            return;
        PileType? type = cardPlay.Card.Pile?.Type;
        if (type == PileType.Hand || type == PileType.Play)
        {
            await PowerCmd.Decrement(this);
        }
    }
}
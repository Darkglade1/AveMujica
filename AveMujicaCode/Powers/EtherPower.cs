using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class EtherPower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
    public override bool TryModifyEnergyCostInCombatLate(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        if (card.Owner.Creature == Owner)
        {
            if (card.Keywords.Contains(CardKeyword.Exhaust) || card.Keywords.Contains(CardKeyword.Retain))
            {
                modifiedCost = 0M;
                return true;
            }
        }
        modifiedCost = originalCost;
        return false;
    }
    
    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        PileType pileType,
        CardPilePosition position)
    {
        if (card.Owner.Creature == Owner && card.Keywords.Contains(CardKeyword.Retain))
        {
            return (PileType.Exhaust, position);
        }
        return (pileType, position);
    }
}
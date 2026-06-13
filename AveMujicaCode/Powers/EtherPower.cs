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
        if (card.Owner.Creature != Owner)
        {
            modifiedCost = originalCost;
            return false;
        }
        if (card.Keywords.Contains(CardKeyword.Retain))
        {
            card.AddKeyword(CardKeyword.Exhaust);
        }
        if (card.Keywords.Contains(CardKeyword.Exhaust))
        {
            modifiedCost = 0M;
            return true;
        }
        modifiedCost = originalCost;
        return false;
    }
}
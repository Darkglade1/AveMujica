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
    
    public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
    {
        // Don't add Exhaust to Powers since that makes them actually Exhaust instead of disappearing.
        if (card.Owner == Owner.Player && keywords.Contains(CardKeyword.Retain) && card.Type != CardType.Power)
        {
            return keywords.Add(CardKeyword.Exhaust);
        }
        return false;
    }
}
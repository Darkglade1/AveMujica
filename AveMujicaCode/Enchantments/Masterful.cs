using AveMujica.AveMujicaCode.Cards.CardMods;
using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Enchantments;

public class Masterful : AveMujicaEnchantment
{
    public override bool HasExtraCardText => true;
    public override bool ShowAmount => true;
    public override bool IsStackable => true;
    
    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && (card.Type == CardType.Attack || card.GainsBlock || card is Song);
    }

    public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
    {
        return !props.IsPoweredAttack() ? 0 : Amount;
    }
    
    public override Decimal EnchantBlockAdditive(Decimal originalBlock)
    {
        return Amount;
    }

    public static void TryEnchantCardWithMasterful(CardModel card, int amount)
    {
        var enchantment = ModelDb.Enchantment<Masterful>();
        if (enchantment.CanEnchant(card))
        {
            CardCmd.Enchant<Masterful>(card, amount);
        }
        else
        {
            var masterfulMod = (MasterfulMod)ModelDb.Get<MasterfulMod>().MutableClone();
            masterfulMod.Amount = amount;
            CardModifier.AddModifier(card, masterfulMod);
        }
    }
}
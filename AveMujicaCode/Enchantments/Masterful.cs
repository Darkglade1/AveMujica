using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Enchantments;

public class Masterful : AveMujicaEnchantment
{
    public override bool HasExtraCardText => true;
    public override bool ShowAmount => true;
    public override bool IsStackable => true;

    public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
    {
        return !props.IsPoweredAttack() ? 0 : Amount;
    }
    
    public override Decimal EnchantBlockAdditive(Decimal originalBlock)
    {
        return Amount;
    }
}
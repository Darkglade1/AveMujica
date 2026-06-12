using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;

namespace AveMujica.AveMujicaCode.Enchantments;

public abstract class AveMujicaEnchantment : CustomEnchantmentModel
{
    protected override string CustomIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentPath();
}
using BaseLib.Abstracts;
using BaseLib.Utils;
using AveMujica.AveMujicaCode.Character;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Extensions;

namespace AveMujica.AveMujicaCode.Potions;

[Pool(typeof(AveMujicaPotionPool))]
public abstract class AveMujicaPotion : CustomPotionModel
{
    public override string CustomPackedImagePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionPath();

    public override string CustomPackedOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionPath();
}
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace AveMujica.AveMujicaCode.Cards.Token;

public abstract class AbstractDollCard(int cost, CardType type, CardRarity rarity, TargetType target)
    : AveMujicaCard(cost, type, rarity, target)
{
    public override string CustomPortraitPath => IsSkinEnabled() ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_skin.png".BigCardImagePath() : $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => IsSkinEnabled() ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_skin.png".CardImagePath() : $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected abstract bool IsSkinEnabled();
}
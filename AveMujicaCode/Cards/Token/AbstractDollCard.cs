using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;

namespace AveMujica.AveMujicaCode.Cards.Token;

public abstract class AbstractDollCard(int cost, CardType type, CardRarity rarity, TargetType target)
    : AveMujicaCard(cost, type, rarity, target), ICustomTypeTextCard
{
    public override string CustomPortraitPath => IsSkinEnabled() ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_skin.png".BigCardImagePath() : $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => IsSkinEnabled() ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_skin.png".CardImagePath() : $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected abstract bool IsSkinEnabled();
    public IEnumerable<LocString> GetTypeModifiers()
    {
        var dollType = new LocString("static_hover_tips", "AVEMUJICA-DOLL_TYPE.description");
        return [dollType];
    }
}
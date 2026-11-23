using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Token;

public abstract class AbstractDollCard(int cost, CardType type, CardRarity rarity, TargetType target)
    : AveMujicaCard(cost, type, rarity, target), ICustomTypeTextCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(StartingHp)];
    public override int MaxUpgradeLevel => 0;
    public virtual int StartingHp => 3;
    public bool IsDollPresent;
    public override string CustomPortraitPath => IsSkinEnabled() ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_skin.png".BigCardImagePath() : $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => IsSkinEnabled() ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_skin.png".CardImagePath() : $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected abstract bool IsSkinEnabled();
    public abstract MonsterModel GetAssociatedDoll();
    public IEnumerable<LocString> GetTypeModifiers()
    {
        var dollType = new LocString("static_hover_tips", "AVEMUJICA-DOLL_TYPE.description");
        return [dollType];
    }
    
    protected override void AddExtraArgsToDescription(LocString description)
    {
        description.Add("IsDollPresent", IsDollPresent);
    }
}
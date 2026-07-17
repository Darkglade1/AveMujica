using AveMujica.AveMujicaCode.Cards.CardMods;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class Fortissimo() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ComposeVar(2)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose),
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var strengthMod = (StrengthMod)ModelDb.Get<StrengthMod>().MutableClone();
        strengthMod.Amount = DynamicVars["Compose"].IntValue;
        await ComposeHelper.AddComposeEffectsToSong([strengthMod], Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Compose"].UpgradeValueBy(1);
    }
}
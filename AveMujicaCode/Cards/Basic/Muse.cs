using AveMujica.AveMujicaCode.Cards.CardMods;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Basic;

public class Muse() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await ComposeHelper.RandomCompose(Owner, choiceContext, IsUpgraded);
    }

    protected override void OnUpgrade()
    {

    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose)
    ];
}
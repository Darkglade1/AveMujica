using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AveMujica.AveMujicaCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Doloris() : AllyCard(-1,
    CardType.Power, CardRarity.Token,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
    }

    protected override void OnUpgrade()
    {
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Awaken),
        DolorisDoll.GenerateCardHoverTip()
    ];

    protected override bool IsSkinEnabled()
    {
        return Config.UseDolorisSkin;
    }
}
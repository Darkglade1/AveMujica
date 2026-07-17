using AveMujica.AveMujicaCode.Cards.Dolls;
using BaseLib.Patches.Features;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AveMujica.AveMujicaCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Weave() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Token,
    CustomTargetType.PetOrSelf)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];
    
    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.PerformsDreamspin];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
        if (IsUpgraded)
        {
            await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
        }
    }

    protected override void OnUpgrade()
    {
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin)
    ];
}
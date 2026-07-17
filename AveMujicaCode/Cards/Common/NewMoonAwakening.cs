using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class NewMoonAwakening() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Common,
    CustomTargetType.PetOrSelf)
{
    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.PerformsDreamspin];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
        await CardPileCmd.AddToCombatAndPreview<Weave>(Owner.Creature, PileType.Draw, 1, Owner, CardPilePosition.Random);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin),
        HoverTipFactory.FromCard<Weave>()
    ];
}
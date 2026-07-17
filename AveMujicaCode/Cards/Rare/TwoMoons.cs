using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class TwoMoons() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin),
        HoverTipFactory.FromCard<Weave>(),
    ];
    
    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.PerformsDreamspin];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
        await CardPileCmd.AddToCombatAndPreview<Weave>(Owner.Creature, PileType.Hand, 1, Owner);
    }
    
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
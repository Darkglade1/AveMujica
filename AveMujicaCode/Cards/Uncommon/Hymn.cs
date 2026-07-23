using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class Hymn() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.PerformsDreamspin];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<HymnPower>(choiceContext, Owner.Creature,1, Owner.Creature, this);
        await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin),
        DolorisDoll.GenerateCardHoverTip(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
}
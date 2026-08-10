using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class TwoMoons() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Rare,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin),
        HoverTipFactory.FromCard<Weave>(IsUpgraded),
    ];
    
    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.PerformsDreamspin];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
        if (Owner.Creature.CombatState != null)
        {
            CardModel card = Owner.Creature.CombatState.CreateCard<Weave>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }
    
    protected override void OnUpgrade()
    {
    }
}
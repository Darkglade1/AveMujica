using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class BandRecruitment() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Awaken)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        List<CardModel> bandMembers =
        [
            (CardModel)ModelDb.Card<Doloris>().MutableClone(),
            (CardModel)ModelDb.Card<Mortis>().MutableClone(),
            (CardModel)ModelDb.Card<Timoris>().MutableClone(),
            (CardModel)ModelDb.Card<Amoris>().MutableClone()
        ];
        foreach (CardModel card in bandMembers)
        {
            if (Owner.Creature.CombatState != null)
            {
                Owner.Creature.CombatState.AddCard(card, Owner);
            }
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }
        }
        bandMembers.StableShuffle(Owner.RunState.Rng.CombatCardGeneration);
        bandMembers.RemoveAt(bandMembers.Count - 1);
        var pickedCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, bandMembers, Owner);
        if (pickedCard != null)
        {
            await CardPileCmd.AddGeneratedCardToCombat(pickedCard, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade()
    {

    }
}
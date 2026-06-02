using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class NewMoonAwakening() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Oblivion>(4), new CardsVar(1)];

    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.GainsOblivion];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<Oblivion>(choiceContext, Owner.Creature, DynamicVars["Oblivion"].BaseValue, Owner.Creature, this);
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, (int)DynamicVars.Cards.BaseValue);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (card == null)
            return;
        await CardCmd.Exhaust(choiceContext, card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Oblivion"].UpgradeValueBy(2);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower(ModelDb.Power<Oblivion>()),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];
}
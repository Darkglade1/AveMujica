using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Cards.Uncommon;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Ancient;

public class Knights() : AveMujicaCard(3,
    CardType.Power, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(2)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Repeat.IntValue);
        var allies = AllyHelper.AllyCardList();
        var selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, allies, Owner, prefs);
        foreach (CardModel selectedCard in selectedCards)
        {
            if (selectedCard is Doloris)
            {
                await AllyHelper.Awaken<DolorisAlly>(choiceContext, Owner, DolorisAlly.StartingHP, this);
            }
            if (selectedCard is Mortis)
            {
                await AllyHelper.Awaken<MortisAlly>(choiceContext, Owner, MortisAlly.StartingHP, this);
            }
            if (selectedCard is Timoris)
            {
                await AllyHelper.Awaken<TimorisAlly>(choiceContext, Owner, TimorisAlly.StartingHP, this);
            }
            if (selectedCard is Amoris)
            {
                await AllyHelper.Awaken<AmorisAlly>(choiceContext, Owner, AmorisAlly.StartingHP, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Awaken)
    ];
}
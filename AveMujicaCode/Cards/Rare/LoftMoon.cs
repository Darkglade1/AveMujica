using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Cards.Uncommon;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class LoftMoon() : AveMujicaCard(3,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<LoftMoonPower>(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
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
        await PowerCmd.Apply<LoftMoonPower>(choiceContext, Owner.Creature, DynamicVars["LoftMoonPower"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Awaken)
    ];
}
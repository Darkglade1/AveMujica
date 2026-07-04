using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class MaidensCapriccio() : AbstractPerformCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new("BaseExhaust", 2), new("Exhaust", 2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Draw(this, choiceContext);
        await ExecutePerformEffect(choiceContext, play, PerformSequences()[0]);
    }
    
    protected override List<CardType[]> PerformSequences()
    {
        CardType[] cardTypes = [CardType.Skill];
        return [cardTypes];
    }
    
    protected override async Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes, int numTriggers)
    {
        DynamicVars["Exhaust"].BaseValue = DynamicVars["BaseExhaust"].IntValue * numTriggers;
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars["Exhaust"].IntValue);
        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this))
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
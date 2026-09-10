using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Enchantments;
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
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new("Enchant", 4)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> items = new List<IHoverTip>();
            items.Add(HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform));
            items.AddRange(HoverTipFactory.FromEnchantment<Masterful>());
            return items;
        }
    }

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
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel? selection = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, (Func<CardModel, bool>) (c => c.Type == CardType.Attack || c.GainsBlock || c is Song), this)).FirstOrDefault();
        if (selection != null)
        {
            Masterful.TryEnchantCardWithMasterful(selection, DynamicVars["Enchant"].IntValue * numTriggers);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars["Enchant"].UpgradeValueBy(1);
    }
}
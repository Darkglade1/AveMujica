using AveMujica.AveMujicaCode.Cards.Allies;
using BaseLib.Patches.Features;
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
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DreamspinVar(4), new CardsVar(1)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await AllyHelper.Dreamspin(choiceContext, Owner, DynamicVars["Dreamspin"].IntValue, play.Target, this);
        if (IsUpgraded)
        {
            CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars.Cards.IntValue);
            CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
            if (card == null)
                return;
            await CardCmd.Exhaust(choiceContext, card);
        }
        else
        {
            CardPile pile = PileType.Hand.GetPile(Owner);
            CardModel? card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
            if (card == null)
                return;
            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Dreamspin"].UpgradeValueBy(1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];
}
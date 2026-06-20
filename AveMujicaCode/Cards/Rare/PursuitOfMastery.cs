using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Enchantments;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class PursuitOfMastery() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Enchant",3), new("EnchantGrow", 2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Masterful>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel? selection = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, (Func<CardModel, bool>) (c => c.Type == CardType.Attack || c.GainsBlock || c is Song), this)).FirstOrDefault();
        if (selection != null)
        {
            Masterful.TryEnchantCardWithMasterful(selection, DynamicVars["Enchant"].IntValue);
        }
    }
    
    public override Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player && PileType.Hand.GetPile(Owner).Cards.Contains(this))
        {
            DynamicVars["Enchant"].BaseValue += DynamicVars["EnchantGrow"].BaseValue;
        }
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Enchant"].UpgradeValueBy(1);
        DynamicVars["EnchantGrow"].UpgradeValueBy(1);
    }
}
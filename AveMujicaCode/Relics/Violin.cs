using AveMujica.AveMujicaCode.Enchantments;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Relics;

public class Violin() : AveMujicaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new("Masterful", 4)];
    
    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Masterful>(DynamicVars["Masterful"].IntValue);
    
    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        Masterful canonicalEnchantment = ModelDb.Enchantment<Masterful>();
        foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(Owner, canonicalEnchantment, DynamicVars["Masterful"].IntValue, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, DynamicVars["Masterful"].IntValue);
            CardCmd.Preview(card);
        }
    }
}
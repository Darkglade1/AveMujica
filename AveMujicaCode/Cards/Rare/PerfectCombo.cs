using AveMujica.AveMujicaCode.Enchantments;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class PerfectCombo() : AveMujicaCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Masterful", 3)];

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
        await PowerCmd.Apply<PerfectComboPower>(choiceContext, Owner.Creature, DynamicVars["Masterful"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
         DynamicVars["Masterful"].UpgradeValueBy(2);
    }
}
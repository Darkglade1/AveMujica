using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class HowDareYou() : AveMujicaCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<HowDareYouPower>(6)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        DolorisDoll.GenerateCardHoverTip(),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<HowDareYouPower>(choiceContext, Owner.Creature, DynamicVars["HowDareYouPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["HowDareYouPower"].UpgradeValueBy(3);
    }
}
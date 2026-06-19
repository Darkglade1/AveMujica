using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class InspiringDance() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];
    
    protected override bool HasEnergyCostX => true;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int num = ResolveEnergyXValue();
        if (IsUpgraded)
        {
            num++;
        }
        if (num > 0 && play.Target != null && play.Target.Player != null)
        {
            await PlayerCmd.GainEnergy(num, play.Target.Player);
            await CardPileCmd.Draw(choiceContext, num, play.Target.Player);
        }
    }

    protected override void OnUpgrade()
    {
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        EnergyHoverTip
    ];
}
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class WaxingMoonLonging() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((c, _) 
            => c.IsUpgraded ? c.Owner.Creature.GetPowerAmount<Oblivion>() + 3 : c.Owner.Creature.GetPowerAmount<Oblivion>())
    ];
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(play.Target), DynamicVars.CalculatedBlock.Props, play);
    }

    protected override void OnUpgrade()
    {

    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower(ModelDb.Power<Oblivion>())
    ];
}
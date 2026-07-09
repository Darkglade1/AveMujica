using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class Rehearsal() : AbstractPerformCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
        await ExecutePerformEffect(choiceContext, play, PerformSequences()[0]);
    }
    
    protected override List<CardType[]> PerformSequences()
    {
        CardType[] cardTypes = [CardType.Attack];
        return [cardTypes];
    }
    
    protected override async Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes, int numTriggers)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue * numTriggers, Owner);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform),
        EnergyHoverTip
    ];
}
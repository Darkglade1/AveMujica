using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;
public class Anthem() : AbstractPerformCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AnthemPower>(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform)
    ];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<NextTurnAnthemPower>(choiceContext, Owner.Creature, DynamicVars["AnthemPower"].BaseValue, Owner.Creature, this);
        await ExecutePerformEffect(choiceContext, play, PerformSequences()[0]);
    }
    
    protected override List<CardType[]> PerformSequences()
    {
        CardType[] cardTypes = [CardType.Attack, CardType.Skill];
        return [cardTypes];
    }
    
    protected override async Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes, int numTriggers)
    {
        await PowerCmd.Apply<NextTurnAnthemPower>(choiceContext, Owner.Creature, DynamicVars["AnthemPower"].BaseValue * numTriggers, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["AnthemPower"].UpgradeValueBy(1);
    }
}
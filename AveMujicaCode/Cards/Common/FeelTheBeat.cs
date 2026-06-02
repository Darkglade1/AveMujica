using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class FeelTheBeat() : RhythmCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move), new CardsVar(2)];
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        await ExecuteRhythmEffect(choiceContext, play, RhythmSequences()[0]);
    }
    
    protected override List<CardType[]> RhythmSequences()
    {
        CardType[] cardTypes = [CardType.Skill];
        return [cardTypes];
    }
    
    protected override async Task DoRhythmEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes)
    {
        await CommonActions.Draw(this, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
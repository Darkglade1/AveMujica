using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class GeorgetteMe() : PerformCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move)];
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        await ExecutePerformEffect(choiceContext, play, PerformSequences()[0]);
    }

    protected override List<CardType[]> PerformSequences()
    {
        CardType[] cardTypes = [CardType.Skill, CardType.Skill];
        return [cardTypes];
    }

    protected override async Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes, int numTriggers)
    {
        for (int i = 0; i < numTriggers; i++)
        {
            await CreatureCmd.GainBlock(Owner.Creature, Owner.Creature.Block, ValueProp.Unpowered | ValueProp.Move, play);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3);
}
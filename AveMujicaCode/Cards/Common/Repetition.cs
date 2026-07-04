using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class Repetition() : PerformCard(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (IsPerformActive())
        {
            await ExecutePerformEffect(choiceContext, play, PerformSequences()[0]);
        }
        else
        {
            await CommonActions.CardAttack(this, play).Execute(choiceContext);
        }
    }

    protected override List<CardType[]> PerformSequences()
    {
        CardType[] cardTypes = [CardType.Attack, CardType.Skill];
        return [cardTypes];
    }

    protected override async Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes, int numTriggers)
    {
        await CommonActions.CardAttack(this, play, numTriggers + 1).Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2);
}
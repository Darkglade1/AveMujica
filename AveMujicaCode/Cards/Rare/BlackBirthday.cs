using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class BlackBirthday() : PerformCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new RepeatVar(2)];

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
        CardType[] cardTypes = [CardType.Attack, CardType.Attack];
        return [cardTypes];
    }

    protected override async Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes, int numTriggers)
    {
        await CommonActions.CardAttack(this, play, (DynamicVars.Repeat.IntValue * numTriggers) + 1).Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);
}
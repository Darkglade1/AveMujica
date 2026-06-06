using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class WhiteKeysBlackKeys() : PerformCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2), new PowerVar<VulnerablePower>(2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await ExecutePerformEffect(choiceContext, play, PerformSequences()[0]);
        await ExecutePerformEffect(choiceContext, play, PerformSequences()[1]);
    }
    
    protected override List<CardType[]> PerformSequences()
    {
        CardType[] cardTypes1 = [CardType.Skill];
        CardType[] cardTypes2 = [CardType.Attack];
        return [cardTypes1, cardTypes2];
    }
    
    protected override async Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes)
    {
        if (cardTypes[0] == CardType.Skill)
        {
            ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
            await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this);
        }
        if (cardTypes[0] == CardType.Attack)
        {
            ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
            await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Cards.Token;

public class Song() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Token,
    TargetType.AnyEnemy)
{
    public bool IsAttack = false;
    public bool IsTargeted = false;
    public override CardType Type => IsAttack ? CardType.Attack : CardType.Skill;
    public override TargetType TargetType => IsTargeted ? TargetType.AnyEnemy : TargetType.Self;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
    }

    protected override void OnUpgrade()
    {

    }
}
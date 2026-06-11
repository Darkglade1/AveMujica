using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AveMujica.AveMujicaCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
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
        if (IsUpgraded)
        {
            await CardPileCmd.Draw(choiceContext, 1, Owner);
        }
    }

    protected override void OnUpgrade()
    {

    }
}
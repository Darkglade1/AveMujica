using AveMujica.AveMujicaCode.Hooks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class EndlessPerformance() : AveMujicaCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy), IAfterPerform
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }

    public async Task AfterPerform(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Card.Owner != Owner || Pile?.Type == PileType.Hand || Pile?.Type == PileType.Deck)
        {
            return;
        }
        await CardPileCmd.Add(this, PileType.Hand);
    }
}
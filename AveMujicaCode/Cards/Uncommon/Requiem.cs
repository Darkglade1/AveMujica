using AveMujica.AveMujicaCode.Hooks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class Requiem() : AveMujicaCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies), IAfterPerform
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }

    public Task AfterPerform(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Card.Owner == Owner)
        {
            EnergyCost.SetUntilPlayed(0);
        }
        return Task.CompletedTask;
    }
    
    public override CardLocation ModifyCardPlayResultLocation(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card == this)
        {
            return new CardLocation(card.Owner, PileType.Draw, CardPilePosition.Random);
        }
        return cardLocation;
    }
}
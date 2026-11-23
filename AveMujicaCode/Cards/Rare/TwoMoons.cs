using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class TwoMoons() : AveMujicaCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin),
        HoverTipFactory.FromCard<Weave>(IsUpgraded),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (Owner.Creature.CombatState != null)
        {
            CardModel card = Owner.Creature.CombatState.CreateCard<Weave>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards;

public class Waltz() : AveMujicaCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
 
        CardModel? lastCard = CombatManager.Instance.History.CardPlaysFinished.LastOrDefault()?.CardPlay.Card;
        if (lastCard is { Type: CardType.Attack })
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        }
    }
    
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            CardModel? lastCard = CombatManager.Instance.History.CardPlaysFinished.LastOrDefault()?.CardPlay.Card;
            if (lastCard is { Type: CardType.Attack })
            {
                return true;
            }

            return false;
        }
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Rhythm)
    ];

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2);
}
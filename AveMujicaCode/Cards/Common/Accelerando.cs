using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class Accelerando() : AveMujicaCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move), new EnergyVar(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip, HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override bool ShouldGlowGoldInternal => WasCardExhaustedThisTurn;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (WasCardExhaustedThisTurn)
        {
            await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
    
    public bool WasCardExhaustedThisTurn
    {
        get
        {
            return CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner);
        }
    }
}
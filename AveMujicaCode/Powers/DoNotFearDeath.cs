using AveMujica.AveMujicaCode.Cards.Allies;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Powers;

public class DoNotFearDeath : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override Creature ModifyUnblockedDamageTarget(
        Creature target,
        Decimal _,
        ValueProp props,
        Creature? __)
    {
        return target != Owner.PetOwner?.Creature || Owner.IsDead || !props.IsPoweredAttack() ? target : Owner;
    }

    public override bool ShouldAllowHitting(Creature creature) => creature.IsAlive;
    
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (Owner.Monster is MortisAlly && target == Owner)
        {
            await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Owner, result.UnblockedDamage, false);
        }
    }
    
    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Enemy)
        {
            await PowerCmd.Remove<DoNotFearDeath>(Owner);
            if (Owner.Monster is MortisAlly)
            {
                await CreatureCmd.TriggerAnim(Owner, "SwitchOut", 0);
            }
        }
    }
}
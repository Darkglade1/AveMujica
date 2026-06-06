using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Powers;

public class AsFierceAsFire() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            await PowerCmd.Remove<AsFierceAsFire>(Owner);
        }
    }
    
    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (dealer == null || dealer != Owner && dealer.PetOwner?.Creature != Owner || !props.IsPoweredAttack() || result.TotalDamage <= 0)
            return;
        var existing = Owner.CombatState?.Allies.FirstOrDefault(c => c.Monster is AmorisAlly && c.PetOwner == Owner.Player && c.IsAlive);
        if (existing != null && existing.Monster != null)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, existing.Monster.Creature, Amount, Owner, null);
        }
    }

    
}
using AveMujica.AveMujicaCode.Audio;
using AveMujica.AveMujicaCode.Cards.Allies;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Powers;

public class HowDareYou : AveMujicaPower
{
    private bool triggeredAnimThisTurn;
    
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == CombatSide.Player)
        {
            triggeredAnimThisTurn = false;
        }
        return Task.CompletedTask;
    }
    
    public override async Task BeforeDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner || dealer == null || !props.IsPoweredAttack() && !(cardSource is Omnislice))
            return;
        var existing = Owner.CombatState?.Allies.FirstOrDefault(c => c.Monster is DolorisAlly && c.PetOwner == Owner.Player && c.IsAlive);
        if (existing != null && existing.Monster != null)
        {
            Flash();
            if (!triggeredAnimThisTurn)
            {
                await CreatureCmd.TriggerAnim(existing, "Attack", 0);
                Sfx.SKILL_GUITAR_VOCALS3.Play();
                triggeredAnimThisTurn = true;
            }
            await CreatureCmd.Damage(choiceContext, dealer, Amount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, existing, null);
            Amount++;
        }
    }
}
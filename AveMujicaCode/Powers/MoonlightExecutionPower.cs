using MegaCrit.Sts2.Core.Entities.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class MoonlightExecutionPower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    // public override async Task AfterPowerAmountChanged(
    //     PlayerChoiceContext choiceContext,
    //     PowerModel power,
    //     Decimal amount,
    //     Creature? applier,
    //     CardModel? cardSource)
    // {
    //     if (power == this && Owner.CombatState != null)
    //     {
    //         foreach (var ally in Owner.CombatState.Allies)
    //         {
    //             if (ally.Monster is AmorisAlly amoris)
    //             {
    //                 Flash();
    //                 amoris.currentHits = AmorisAlly.baseHits + Amount;
    //                 amoris.SetMoveImmediate(amoris.GetDefaultMoveState());
    //             }
    //         }
    //     }
    // }
    //
    //
    // public Task AfterAwaken(PlayerChoiceContext choiceContext, Player player, AbstractAlly ally)
    // {
    //     if (player.Creature == Owner && ally is AmorisAlly amoris)
    //     {
    //         Flash();
    //         amoris.currentHits = AmorisAlly.baseHits + Amount;
    //         amoris.SetMoveImmediate(amoris.GetDefaultMoveState());
    //     }
    //     return Task.CompletedTask;
    // }
}
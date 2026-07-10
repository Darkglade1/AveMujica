using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Hooks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class AsStrongAsWheatPower() : AveMujicaPower, IAfterAwaken
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (power == this && Owner.CombatState != null)
        {
            foreach (var ally in Owner.CombatState.Allies)
            {
                if (ally.Monster is AmorisDoll amoris && ally.IsAlive && ally.PetOwner?.Creature == Owner)
                {
                    Flash();
                    amoris.currentHits = AmorisDoll.baseHits + Amount;
                    amoris.SetMoveImmediate(amoris.GetDefaultMoveState());
                }
            }
        }
    }


    public Task AfterAwaken(PlayerChoiceContext choiceContext, Player player, AbstractDoll doll)
    {
        if (player.Creature == Owner && doll is AmorisDoll amoris)
        {
            Flash();
            amoris.currentHits = AmorisDoll.baseHits + Amount;
            amoris.SetMoveImmediate(amoris.GetDefaultMoveState());
        }
        return Task.CompletedTask;
    }
}
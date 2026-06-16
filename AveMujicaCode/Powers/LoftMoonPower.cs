using AveMujica.AveMujicaCode.Cards.Allies;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Powers;

public class LoftMoonPower() : AveMujicaPower
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
        if (side == CombatSide.Player && Owner.Player != null && Owner.CombatState != null)
        {
            foreach (var ally in Owner.CombatState.Allies)
            {
                if (ally.Monster is AbstractAlly && ally.IsAlive && ally.PetOwner == Owner.Player)
                {
                    await CreatureCmd.GainMaxHp(ally, Amount);
                }
            }
        }
    }
}
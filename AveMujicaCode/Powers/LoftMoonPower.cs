using AveMujica.AveMujicaCode.Cards.Dolls;
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
    
    public override async Task BeforeSideTurnEndEarly(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player && Owner.Player != null && Owner.CombatState != null)
        {
            Creature? lowestHpDoll = null;
            int lowestHP = 9999;
            foreach (var ally in Owner.CombatState.Allies)
            {
                if (ally.Monster is AbstractDoll && ally.IsAlive && ally.PetOwner == Owner.Player)
                {
                    if (ally.CurrentHp < lowestHP)
                    {
                        lowestHpDoll = ally;
                        lowestHP = ally.CurrentHp;
                    }
                }
            }
            if (lowestHpDoll != null)
            {
                await CreatureCmd.GainMaxHp(lowestHpDoll, Amount);
            }
        }
    }
}
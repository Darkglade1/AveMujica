using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Powers;

public class Oblivion : AveMujicaPower
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
        if (Owner.CombatState != null && power is Oblivion && Owner == applier)
        {
            IReadOnlyList<Creature> hittableEnemies = Owner.CombatState.HittableEnemies;
            if (hittableEnemies.Count != 0)
            {
                Creature? weakestEnemy = hittableEnemies.MinBy((Func<Creature, int>) (c => c.CurrentHp));
                if (weakestEnemy != null)
                {
                    Flash();
                    await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), weakestEnemy, Amount, ValueProp.Unpowered, Owner);
                }
            }
        }
    }
    
    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _)
    {
        var oblivionForm = Owner.GetPower<GoddessOfOblivionPower>();
        if (card.Owner.Creature != Owner || oblivionForm == null)
            return;
        if (Owner.CombatState != null)
        {
            IReadOnlyList<Creature> hittableEnemies = Owner.CombatState.HittableEnemies;
            if (hittableEnemies.Count != 0)
            {
                Creature? weakestEnemy = hittableEnemies.MinBy((Func<Creature, int>) (c => c.CurrentHp));
                if (weakestEnemy != null)
                {
                    Flash();
                    await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), weakestEnemy, Amount, ValueProp.Unpowered, Owner);
                }
            }
        }
    }
}
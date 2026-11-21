using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Powers;

public class Oblivion : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _)
    {
        if (Owner.CombatState != null && Owner.Player == card.Owner)
        {
            IReadOnlyList<Creature> hittableEnemies = Owner.CombatState.HittableEnemies;
            if (hittableEnemies.Count != 0)
            {
                Creature? weakestEnemy = hittableEnemies.MinBy((Func<Creature, int>) (c => c.CurrentHp));
                if (weakestEnemy != null)
                {
                    Flash();
                    var amountOfHpToLose = Hook.ModifyHpLost(Owner.Player.RunState, Owner.CombatState, weakestEnemy,
                        Amount, ValueProp.Unpowered, Owner, null, HpLossHookPhase.AfterOsty, out var _);
                    if (weakestEnemy.CurrentHp + weakestEnemy.Block > amountOfHpToLose)
                    {
                        await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), weakestEnemy, Amount, ValueProp.Unpowered, Owner);
                    }
                    else
                    {
                        Node? vfxContainer = weakestEnemy.GetVfxContainer();
                        NDamageNumVfx? child = NDamageNumVfx.Create(weakestEnemy, (int)amountOfHpToLose);
                        if (child != null)
                        {
                            if (vfxContainer != null)
                            {
                                vfxContainer.AddChildSafely(child);
                            }
                            else
                            {
                                NRun.Instance?.GlobalUi.AddChildSafely(child);
                            }
                        }
                        NGame.Instance?.ScreenShake(ShakeStrength.Medium, ShakeDuration.Short);
                        await DoomPower.PlayVfx(weakestEnemy);
                        await CreatureCmd.Kill(weakestEnemy);
                    }
                }
            }
        }
    }
}
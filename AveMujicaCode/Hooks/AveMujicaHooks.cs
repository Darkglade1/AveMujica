using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace AveMujica.AveMujicaCode.Hooks;

public class AveMujicaHooks
{
    private static async Task DispatchAsync<T>(IRunState? runState, ICombatState? combatState, Func<T, Task> action)
        where T : class
    {
        foreach (var model in runState?.IterateHookListeners(combatState).OfType<T>() ?? [])
        {
            await action(model);
        }
    }

    private static void Dispatch<T>(IRunState? runState, ICombatState? combatState, Action<T> action)
        where T : class
    {
        foreach (var model in runState?.IterateHookListeners(combatState).OfType<T>() ?? [])
        {
            action(model);
        }
    }

    public static Task AfterPerform(IRunState? rs, ICombatState? cs, PlayerChoiceContext choiceContext, CardPlay play)
    {
        return DispatchAsync<IAfterPerform>(rs, cs, m => m.AfterPerform(choiceContext, play));
    }
    
    public static Task OnFinishComposing(IRunState? rs, ICombatState? cs, Player player, CardModel card)
    {
        return DispatchAsync<IOnFinishComposing>(rs, cs, m => m.OnFinishComposing(player, card));
    }
}
using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Hooks;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class GroupPerformancePower() : AveMujicaPower, IOnFinishComposing
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public async Task OnFinishComposing(CardModel card)
    {
        if (Owner.CombatState == null)
        {
            return;
        }
        foreach (Creature creature in Owner.CombatState.GetTeammatesOf(Owner).Where(c => c.IsAlive && c.IsPlayer))
        {
            if (creature.Player != null && creature.CombatState != null)
            {
                for (int i = 0; i < Amount; i++)
                {
                    var copySong = creature.CombatState.CreateCard<Song>(creature.Player);
                    foreach (CardModifier modifier in CardModifier.Modifiers(card))
                    {
                        CardModifier modifier2 = (CardModifier)modifier.MutableClone();
                        copySong.AddModifier(modifier2);
                        modifier2.AfterClonedOnCard(copySong);
                    }
                    await CardPileCmd.AddGeneratedCardToCombat(copySong, PileType.Hand, Owner.Player);
                }
            }
        }
    }
}
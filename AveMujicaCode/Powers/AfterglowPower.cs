using AveMujica.AveMujicaCode.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Powers;
public class AfterglowPower() : AveMujicaPower, IAfterPerform
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public async Task AfterPerform(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Card.Owner.Creature == Owner && Owner.Player != null)
        {
            Flash();
            await PlayerCmd.GainEnergy( Amount, Owner.Player);
            await PowerCmd.Remove<AfterglowPower>(Owner);
        }
    }
}
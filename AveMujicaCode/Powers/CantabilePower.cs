using AveMujica.AveMujicaCode.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Powers;

public class CantabilePower() : AveMujicaPower, IAfterPerform
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public async Task AfterPerform(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Card.Owner.Creature == Owner)
        {
            Flash();
            await CreatureCmd.GainBlock(play.Card.Owner.Creature, Amount, ValueProp.Unpowered, null);
        }
    }
}
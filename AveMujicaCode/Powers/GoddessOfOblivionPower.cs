using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class GoddessOfOblivionPower() : AveMujicaPower
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
        if (card.Owner.Creature == Owner)
        {
            Flash();
            await PowerCmd.Apply<Oblivion>(choiceContext, Owner, Amount, Owner,null);
        }
    }
}
using AveMujica.AveMujicaCode.Cards.Token;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Powers;

public class InspiringSongsPower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner || !(cardPlay.Card is Song))
        {
            return;
        }
        if (Owner.Player != null)
        {
            Flash();
            await PowerCmd.Apply<InspiringSongsTempStrPower>(choiceContext, Owner, Amount, Owner, null);
            await PowerCmd.Apply<InspiringSongsTempDexPower>(choiceContext, Owner, Amount, Owner, null);
        }
    }
}
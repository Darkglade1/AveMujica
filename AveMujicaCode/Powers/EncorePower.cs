using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Hooks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;
public class EncorePower() : AveMujicaPower, IOnFinishComposing
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
        if (Owner.CombatState != null && power is EncorePower && Owner == applier && Owner.Player != null)
        {
            var hand = PileType.Hand.GetPile(Owner.Player);
            foreach (CardModel card in hand.Cards)
            {
                if (card is Song)
                {
                    card._baseReplayCount += (int)amount;
                }
            }
            var drawPile = PileType.Draw.GetPile(Owner.Player);
            foreach (CardModel card in drawPile.Cards)
            {
                if (card is Song)
                {
                    card._baseReplayCount += (int)amount;
                }
            }
            var discardPile = PileType.Discard.GetPile(Owner.Player);
            foreach (CardModel card in discardPile.Cards)
            {
                if (card is Song)
                {
                    card._baseReplayCount += (int)amount;
                }
            }
            var exhaustPile = PileType.Exhaust.GetPile(Owner.Player);
            foreach (CardModel card in exhaustPile.Cards)
            {
                if (card is Song)
                {
                    card._baseReplayCount += (int)amount;
                }
            }
        }
    }
    
    public async Task OnFinishComposing(Player composer, CardModel card)
    {
        if (composer.Creature != Owner || !(card is Song))
        {
            return;
        }
        card._baseReplayCount += Amount;
    }
}
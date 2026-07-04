using AveMujica.AveMujicaCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class FlawlessPerformancePower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (Owner.CombatState != null && power is FlawlessPerformancePower && Owner == applier && Owner.Player != null)
        {
            var hand = PileType.Hand.GetPile(Owner.Player);
            foreach (CardModel card in hand.Cards)
            {
                if (card is AbstractPerformCard)
                {
                    CardCmd.ApplyKeyword(card, CardKeyword.Retain);
                }
            }
            var drawPile = PileType.Draw.GetPile(Owner.Player);
            foreach (CardModel card in drawPile.Cards)
            {
                if (card is AbstractPerformCard)
                {
                    CardCmd.ApplyKeyword(card, CardKeyword.Retain);
                }
            }
            var discardPile = PileType.Discard.GetPile(Owner.Player);
            foreach (CardModel card in discardPile.Cards)
            {
                if (card is AbstractPerformCard)
                {
                    CardCmd.ApplyKeyword(card, CardKeyword.Retain);
                }
            }
            var exhaustPile = PileType.Exhaust.GetPile(Owner.Player);
            foreach (CardModel card in exhaustPile.Cards)
            {
                if (card is AbstractPerformCard)
                {
                    CardCmd.ApplyKeyword(card, CardKeyword.Retain);
                }
            }
        }
    }

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator.Creature != Owner || !(card is AbstractPerformCard))
        {
            return;
        }
        CardCmd.ApplyKeyword(card, CardKeyword.Retain);
    }
}
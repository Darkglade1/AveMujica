using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Enchantments;
using AveMujica.AveMujicaCode.Hooks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;

public class PerfectComboPower : AveMujicaPower, IAfterPerform
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
            List<CardModel> possibleTargets = new List<CardModel>();
            foreach (var card in PileType.Hand.GetPile(play.Player).Cards)
            {
                if (card.Type == CardType.Attack || card.GainsBlock || card is Song)
                {
                    possibleTargets.Add(card);
                }
            }

            if (possibleTargets.Count > 0)
            {
                CardModel? card = play.Player.RunState.Rng.CombatCardSelection.NextItem(possibleTargets);
                if (card != null)
                {
                    Masterful.TryEnchantCardWithMasterful(card, Amount);
                }
            }
        }
    }
}
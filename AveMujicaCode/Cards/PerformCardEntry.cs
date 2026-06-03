using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards;

public class PerformCardEntry : CombatHistoryEntry
{
  public CardModel Card { get; }

  public override string Description => $"{Actor.Player.Character.Id.Entry} Performed {Card.Id.Entry}";

  public PerformCardEntry(
    CardModel card,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(card.Owner.Creature, roundNumber, currentSide, history, players)
  {
    Card = card;
  }
}
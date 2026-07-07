using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace AveMujica.AveMujicaCode.Cards;

public class DreamspinEntry : CombatHistoryEntry
{

  public override string Description => $"{Actor.Player.Character.Id.Entry} Dreamspinned";

  public DreamspinEntry(
    Creature source,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(source, roundNumber, currentSide, history, players)
  {
  }
}
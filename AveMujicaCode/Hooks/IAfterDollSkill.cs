using AveMujica.AveMujicaCode.Cards.Dolls;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Hooks;

public interface IAfterDollSkill
{
    Task AfterDollSkill(Player player, AbstractDoll doll);
}
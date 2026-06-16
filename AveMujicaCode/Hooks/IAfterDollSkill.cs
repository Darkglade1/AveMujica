using AveMujica.AveMujicaCode.Cards.Allies;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Hooks;

public interface IAfterDollSkill
{
    Task AfterDollSkill(Player player, AbstractAlly ally);
}
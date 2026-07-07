using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Hooks;

public interface IAfterDreamspin
{
    Task AfterDreamspin(PlayerChoiceContext choiceContext, Player player);
}
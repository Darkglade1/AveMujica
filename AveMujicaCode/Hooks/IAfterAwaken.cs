using AveMujica.AveMujicaCode.Cards.Dolls;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Hooks;

public interface IAfterAwaken
{
    Task AfterAwaken(PlayerChoiceContext choiceContext, Player player, AbstractDoll doll);
}
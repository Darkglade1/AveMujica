using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Hooks;

public interface IAfterPerform
{
    Task AfterPerform(PlayerChoiceContext choiceContext, CardPlay play);
}
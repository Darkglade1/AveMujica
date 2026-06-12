using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Hooks;

public interface IOnFinishComposing
{
    Task OnFinishComposing(Player player, CardModel card);
}
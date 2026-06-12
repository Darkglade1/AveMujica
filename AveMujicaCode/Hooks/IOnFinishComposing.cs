using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Hooks;

public interface IOnFinishComposing
{
    Task OnFinishComposing(CardModel card);
}
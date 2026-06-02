using AveMujica.AveMujicaCode.Cards.Token;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class SongTempStrPower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Song>();
}
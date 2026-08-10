using AveMujica.AveMujicaCode.Cards.Uncommon;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class InspiringSongsTempDexPower : TemporaryDexterityPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<InspiringSongs>();
    
    public string CustomPackedIconPath => "flex.png".PowerImagePath();
    public string CustomBigIconPath => "flex.png".BigPowerImagePath();

    protected override bool IsPositive => true;
}
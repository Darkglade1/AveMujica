using AveMujica.AveMujicaCode.Cards.Rare;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class AnthemTempStrPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Anthem>();
    
    public string CustomPackedIconPath => "flex.png".PowerImagePath();
    public string CustomBigIconPath => "flex.png".BigPowerImagePath();

    protected override bool IsPositive => true;
}
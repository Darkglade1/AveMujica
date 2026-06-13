using AveMujica.AveMujicaCode.Cards.Common;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class DiminuendoPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Diminuendo>();
    
    public string CustomPackedIconPath => "shackle.png".PowerImagePath();
    public string CustomBigIconPath => "shackle.png".BigPowerImagePath();

    protected override bool IsPositive => false;
}
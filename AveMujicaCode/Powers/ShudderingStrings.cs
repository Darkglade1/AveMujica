using AveMujica.AveMujicaCode.Cards.Uncommon;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class ShudderingStrings : TemporaryStrengthPower, ICustomModel
{
    public override AbstractModel OriginModel => ModelDb.Card<Timoris>();

    protected override bool IsPositive => false;
}
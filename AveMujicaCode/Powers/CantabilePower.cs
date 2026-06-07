using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class CantabilePower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
}
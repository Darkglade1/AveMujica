using MegaCrit.Sts2.Core.Entities.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class FlawlessPerformancePower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
}
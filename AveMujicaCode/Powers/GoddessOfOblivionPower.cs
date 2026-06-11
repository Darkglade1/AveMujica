using MegaCrit.Sts2.Core.Entities.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class GoddessOfOblivionPower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
}
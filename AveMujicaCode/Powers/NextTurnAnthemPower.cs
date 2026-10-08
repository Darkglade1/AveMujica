using AveMujica.AveMujicaCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Powers;

public class NextTurnAnthemPower() : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override string CustomPackedIconPath => "anthem_power.png".PowerImagePath();
    public override string CustomBigIconPath => "anthem_power.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        await PowerCmd.Apply<AnthemPower>(choiceContext, Owner, Amount, Owner, null);
        await PowerCmd.Remove(this);
    }
}
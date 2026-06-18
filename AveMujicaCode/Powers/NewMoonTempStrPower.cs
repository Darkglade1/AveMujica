using AveMujica.AveMujicaCode.Cards.Common;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Powers;
public class NewMoonTempStrPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<NewMoonAwakening>();
    
    public string CustomPackedIconPath => "flex.png".PowerImagePath();
    public string CustomBigIconPath => "flex.png".BigPowerImagePath();

    protected override bool IsPositive => true;
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            Flash();
            await PowerCmd.Remove(this);
            await PowerCmd.Apply<StrengthPower>(choiceContext,Owner,-Sign * Amount,Owner,null);
        }
    }
}
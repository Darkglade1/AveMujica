using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Relics;

public class PlayfulCoretton() : AveMujicaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DreamThreadPower>(3)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DreamThreadPower>()];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.PlayerCombatState?.TurnNumber > 1)
        {
            return;
        }
        Flash();
        await PowerCmd.Apply<DreamThreadPower>(choiceContext, Owner.Creature, DynamicVars["DreamThreadPower"].IntValue, Owner.Creature, null);
    }
}
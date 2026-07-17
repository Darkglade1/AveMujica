using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Cards.CardMods;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Relics;
public class GrandPiano() : AveMujicaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ComposeVar(8), new("Turns", 3)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (Owner.PlayerCombatState == null || side != CombatSide.Player)
        {
            return;
        }

        if (Owner.PlayerCombatState.TurnNumber <= DynamicVars["Turns"].BaseValue)
        {
            Flash();
            var blockMod = (BlockMod)ModelDb.Get<BlockMod>().MutableClone();
            blockMod.Amount = (int)DynamicVars["Compose"].BaseValue;
            await ComposeHelper.AddComposeEffectsToSong([blockMod], Owner);
        }
    }
}
using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Cards.CardMods;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Relics;

public class Keyboard() : AveMujicaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<GrandPiano>();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ComposeVar(8)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task BeforeCombatStart()
    {
        Flash();
        var blockMod = (BlockMod)ModelDb.Get<BlockMod>().MutableClone();
        blockMod.BlockVar = new BlockVar((int)DynamicVars["Compose"].BaseValue, ValueProp.Move);
        await ComposeHelper.AddComposeEffectsToSong([blockMod], Owner);
    }
}
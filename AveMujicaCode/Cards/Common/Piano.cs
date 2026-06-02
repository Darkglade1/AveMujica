using AveMujica.AveMujicaCode.Cards.CardMods;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class Piano() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7, ValueProp.Move), new ComposeVar(5)];
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    { 
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        var blockMod = (BlockMod)ModelDb.Get<BlockMod>().MutableClone();
        blockMod.BlockAmt = (int)DynamicVars["Compose"].BaseValue;
        await ComposeHelper.AddComposeEffectsToSong([blockMod], Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars["Compose"].UpgradeValueBy(2);
    }
}
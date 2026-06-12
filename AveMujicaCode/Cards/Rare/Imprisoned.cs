using AveMujica.AveMujicaCode.Cards.CardMods;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class Imprisoned() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1), new CardsVar(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose),
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var loseStrengthMod = (LoseStrengthMod)ModelDb.Get<LoseStrengthMod>().MutableClone();
        loseStrengthMod.StrengthAmt = (int)DynamicVars["StrengthPower"].BaseValue;
        await ComposeHelper.AddComposeEffectsToSong([loseStrengthMod], Owner);
        
        var seekMod = (SeekMod)ModelDb.Get<SeekMod>().MutableClone();
        seekMod.SeekAmt = DynamicVars.Cards.IntValue;
        await ComposeHelper.AddComposeEffectsToSong([seekMod], Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
using AveMujica.AveMujicaCode.Cards.CardMods;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class Sophie() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Oblivion>(10), new PowerVar<VulnerablePower>(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose),
        HoverTipFactory.FromPower<Oblivion>(),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var gainVulnerableMod = (GainVulnerableMod)ModelDb.Get<GainVulnerableMod>().MutableClone();
        gainVulnerableMod.VulnerableAmt = (int)DynamicVars["VulnerablePower"].BaseValue;
        await ComposeHelper.AddComposeEffectsToSong([gainVulnerableMod], Owner);
        
        var oblivionMod = (OblivionMod)ModelDb.Get<OblivionMod>().MutableClone();
        oblivionMod.OblivionAmt = (int)DynamicVars["Oblivion"].BaseValue;
        await ComposeHelper.AddComposeEffectsToSong([oblivionMod], Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["VulnerablePower"].UpgradeValueBy(-1);
    }
}
using AveMujica.AveMujicaCode.Cards.CardMods;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class ACappella() : AveMujicaCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(13, ValueProp.Move), new EnergyVar(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose),
        EnergyHoverTip
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        var energyMod = (GainEnergyMod)ModelDb.Get<GainEnergyMod>().MutableClone();
        energyMod.GainEnergyAmt = (int)DynamicVars.Energy.BaseValue;
        await ComposeHelper.AddComposeEffectsToSong([energyMod], Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
}
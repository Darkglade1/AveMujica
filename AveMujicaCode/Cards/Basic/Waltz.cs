using AveMujica.AveMujicaCode.Cards.CardMods;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Basic;

public class Waltz() : AveMujicaCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move), new ComposeVar(6)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        var damageMod = (DamageMod)ModelDb.Get<DamageMod>().MutableClone();
        damageMod.DamageVar = new DamageVar(DynamicVars["Compose"].BaseValue, ValueProp.Move);
        await ComposeHelper.AddComposeEffectsToSong([damageMod], Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Compose"].UpgradeValueBy(2);
    } 
}
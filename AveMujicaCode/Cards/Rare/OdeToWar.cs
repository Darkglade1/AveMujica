using AveMujica.AveMujicaCode.Cards.CardMods;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class OdeToWar() : AveMujicaCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move), new ComposeVar(8)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (Owner.Creature.CombatState != null)
        {
            var numComposeDict = ComposeHelper.ComposeFields.CurrentComposeNum.Get(Owner.Creature.CombatState);
            if (numComposeDict == null)
            {
                numComposeDict = new Dictionary<Player, int>();
            }
            var numComposes = numComposeDict.GetValueOrDefault(Owner);
            var numRemainingComposes = ComposeHelper.NumComposesSongComplete - numComposes;
            for (int i = 0; i < numRemainingComposes; i++)
            {
                var damageMod = (DamageMod)ModelDb.Get<DamageMod>().MutableClone();
                damageMod.Amount = DynamicVars["Compose"].IntValue;
                await ComposeHelper.AddComposeEffectsToSong([damageMod], Owner);
            }
        }
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Compose"].UpgradeValueBy(2);
    }
}
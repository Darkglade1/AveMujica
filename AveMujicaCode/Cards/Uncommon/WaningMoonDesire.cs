using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class WaningMoonDesire() : AveMujicaCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move), new HealVar(2)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (CombatState != null)
        {
            var existing = CombatState.Allies.FirstOrDefault(c => c.Monster is AbstractAlly && c.PetOwner == Owner && c.IsAlive);
            if (existing != null)
            {
                await CreatureCmd.GainMaxHp(existing, DynamicVars.Heal.BaseValue);
            }
        }
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Heal.UpgradeValueBy(1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll)
    ];
}
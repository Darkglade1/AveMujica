using AveMujica.AveMujicaCode.Cards.Dolls;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class BloodMoon() : AveMujicaCard(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move), new HealVar(1)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        Creature? lowestHpDoll = null;
        int lowestHP = 9999;
        if (CombatState != null)
        {
            foreach (var doll in CombatState.Allies)
            {
                if (doll.Monster is AbstractDoll && doll.PetOwner == Owner && doll.IsAlive)
                {
                    if (doll.CurrentHp < lowestHP)
                    {
                        lowestHpDoll = doll;
                        lowestHP = doll.CurrentHp;
                    }
                }
            }
        }
        if (lowestHpDoll != null)
        {
            await CreatureCmd.GainMaxHp(lowestHpDoll, DynamicVars.Heal.BaseValue);
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
using AveMujica.AveMujicaCode.Cards.Dolls;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class CrescentMoonEchos() : AveMujicaCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        AmorisDoll.GenerateCardHoverTip(),
        HoverTipFactory.FromKeyword(AveMujicaKeywords.DollSkill)
    ];
    
    protected override bool HasEnergyCostX => true;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int num = ResolveEnergyXValue();
        int skillNum = num;
        if (IsUpgraded)
        {
            skillNum++;
        }
        await CommonActions.CardAttack(this, play, num).Execute(choiceContext);
        if (CombatState != null)
        {
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is AmorisDoll amoris && ally.PetOwner == Owner && ally.IsAlive)
                {
                    for (int i = 0; i < skillNum; i++)
                    {
                        await amoris.Skill();
                    }
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}
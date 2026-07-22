using AveMujica.AveMujicaCode.Cards.Dolls;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class CrescentMoonEchos() : AveMujicaCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new HpLossVar(2), new("SkillRepeat", 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        AmorisDoll.GenerateCardHoverTip()
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (CombatState != null)
        {
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is AmorisDoll amoris && ally.PetOwner == Owner && ally.IsAlive && ally.CurrentHp >= DynamicVars.HpLoss.BaseValue)
                {
                    for (int i = 0; i < DynamicVars["SkillRepeat"].IntValue; i++)
                    {
                        await amoris.Skill();
                    }
                    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), ally, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, ally);
                    await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), ally, DynamicVars.HpLoss.BaseValue, false);
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["SkillRepeat"].UpgradeValueBy(1);
    }
}
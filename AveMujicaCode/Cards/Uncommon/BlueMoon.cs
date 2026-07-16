using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class BlueMoon() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, ValueProp.Move), new HpLossVar(2), new("SkillRepeat", 1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Doloris>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        if (CombatState != null)
        {
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is DolorisDoll doloris && ally.PetOwner == Owner && ally.IsAlive && ally.CurrentHp >= DynamicVars.HpLoss.BaseValue)
                {
                    for (int i = 0; i < DynamicVars["SkillRepeat"].IntValue; i++)
                    {
                        await doloris.Skill();
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
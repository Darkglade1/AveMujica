using AveMujica.AveMujicaCode.Cards.Dolls;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class TwoMoons() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
        await SpendHPToUseSkill(choiceContext, play);
        if (IsUpgraded)
        {
            await SpendHPToUseSkill(choiceContext, play);
        }
    }

    private async Task SpendHPToUseSkill(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null && play.Target.Monster is AbstractDoll doll && play.Target.CurrentHp >= DynamicVars.HpLoss.BaseValue)
        {
            await doll.Skill();
            await CreatureCmd.Damage(choiceContext, play.Target, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Owner.Creature);
            await CreatureCmd.LoseMaxHp(choiceContext, play.Target, DynamicVars.HpLoss.BaseValue, false);
        }
    }
}
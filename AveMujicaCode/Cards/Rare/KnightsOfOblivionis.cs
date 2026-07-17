using AveMujica.AveMujicaCode.Cards.Dolls;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class KnightsOfOblivionis() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState != null)
        {
            // Make Timoris Skill go first
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is TimorisDoll timoris && ally.PetOwner == Owner && ally.IsAlive && ally.CurrentHp >= DynamicVars.HpLoss.BaseValue)
                {
                    await timoris.Skill();
                    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), ally, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, ally);
                    await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), ally, DynamicVars.HpLoss.BaseValue, false);
                    break;
                }
            }
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is AbstractDoll doll && !(ally.Monster is TimorisDoll) && ally.PetOwner == Owner && ally.IsAlive && ally.CurrentHp >= DynamicVars.HpLoss.BaseValue)
                {
                    await doll.Skill();
                    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), ally, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, ally);
                    await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), ally, DynamicVars.HpLoss.BaseValue, false);
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
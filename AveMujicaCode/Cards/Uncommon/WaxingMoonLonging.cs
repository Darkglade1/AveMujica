using AveMujica.AveMujicaCode.Cards.Allies;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class WaxingMoonLonging() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier(MultiplierCalc)
    ];

    private static decimal MultiplierCalc(CardModel card, Creature? creature)
    {
        if (card.Owner.Creature.CombatState == null)
        {
            return 0;
        }

        int totalHP = 0;
        foreach (var ally in card.Owner.Creature.CombatState.Allies)
        {
            if (ally.IsPet && ally.IsAlive && ally.PetOwner == card.Owner && ally.Monster is AbstractAlly)
            {
                totalHP += ally.CurrentHp;
            }
        }
        return totalHP;
    }
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(play.Target), DynamicVars.CalculatedBlock.Props, play);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll)
    ];
}
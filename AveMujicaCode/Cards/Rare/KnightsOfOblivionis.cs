using AveMujica.AveMujicaCode.Cards.Dolls;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class KnightsOfOblivionis() : AveMujicaCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.Creature.CombatState != null)
        {
            foreach (var ally in Owner.Creature.CombatState.Allies)
            {
                if (ally.IsPet && ally.IsAlive && ally.PetOwner == Owner && ally.Monster is AbstractDoll)
                {
                    await PowerCmd.Apply<StrengthPower>(choiceContext, ally, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
                    await PowerCmd.Apply<DexterityPower>(choiceContext, ally, DynamicVars["StrengthPower"].BaseValue, Owner.Creature,this);   
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
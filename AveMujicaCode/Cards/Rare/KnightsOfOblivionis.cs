using AveMujica.AveMujicaCode.Cards.Dolls;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class KnightsOfOblivionis() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PerformSkills();
        if (CombatState != null && IsUpgraded)
        {
            int numDolls = 0;
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is AbstractDoll && ally.PetOwner == Owner && ally.IsAlive)
                {
                    numDolls++;
                }
            }
            if (numDolls >= 4)
            {
                await PerformSkills();
            }
        }
    }
    
    private async Task PerformSkills()
    {
        if (CombatState != null)
        {
            // Make Timoris Skill go first
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is TimorisDoll timoris && ally.PetOwner == Owner && ally.IsAlive)
                {
                    await timoris.Skill();
                    break;
                }
            }
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is AbstractDoll doll && !(ally.Monster is TimorisDoll) && ally.PetOwner == Owner && ally.IsAlive)
                {
                    await doll.Skill();
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}
using AveMujica.AveMujicaCode.Cards.Dolls;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class KnightsOfOblivionis() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll),
        HoverTipFactory.FromKeyword(AveMujicaKeywords.DollSkill)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PerformSkills(false);
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
                await PerformSkills(true);
            }
        }
    }
    
    private async Task PerformSkills(bool healFirst)
    {
        if (CombatState != null)
        {
            // Make Timoris Skill go first
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is TimorisDoll timoris && ally.PetOwner == Owner && ally.IsAlive)
                {
                    if (healFirst)
                    {
                        await CreatureCmd.GainMaxHp(ally, DynamicVars.Heal.BaseValue);
                    }
                    await timoris.Skill();
                    break;
                }
            }
            foreach (var ally in CombatState.Allies)
            {
                if (ally.Monster is AbstractDoll doll && !(ally.Monster is TimorisDoll) && ally.PetOwner == Owner && ally.IsAlive)
                {
                    if (healFirst)
                    {
                        await CreatureCmd.GainMaxHp(ally, DynamicVars.Heal.BaseValue);
                    }
                    await doll.Skill();
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}
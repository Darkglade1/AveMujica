using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Cards.Token;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class DoNotFearDeath() : AveMujicaCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.Creature.CombatState != null)
        {
            foreach (var ally in Owner.Creature.CombatState.Allies)
            {
                if (ally.Monster is MortisDoll mortis && ally.IsAlive)
                {
                    await mortis.DoNotFearDeath();
                }
            }
        }
        
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Mortis>()
    ];
}
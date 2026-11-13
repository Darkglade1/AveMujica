using AveMujica.AveMujicaCode.Cards.Token;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class CollectiveDream() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin),
        HoverTipFactory.FromCard<Weave>(),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState != null)
        {
            foreach (Player player in CombatState.Players.Where(p => p.Creature.IsAlive && p != Owner))
            {
                if (player.Creature.CombatState != null)
                {
                    for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
                    {
                        var weave = player.Creature.CombatState.CreateCard<Weave>(player);
                        await CardPileCmd.AddGeneratedCardToCombat(weave, PileType.Hand, Owner);
                    }
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
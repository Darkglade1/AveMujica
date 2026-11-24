using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Enchantments;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class VirtuosoBallad() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Masterful",5)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Masterful>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target != null && play.Target.Player != null)
        {
            foreach (var card in PileType.Hand.GetPile(play.Target.Player).Cards)
            {
                if (card.Type == CardType.Attack || card.GainsBlock || card is Song)
                {
                    Masterful.TryEnchantCardWithMasterful(card, DynamicVars["Masterful"].IntValue);
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Masterful"].UpgradeValueBy(2);
    }
}
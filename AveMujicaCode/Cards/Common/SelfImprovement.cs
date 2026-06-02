using AveMujica.AveMujicaCode.Cards.CardMods;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class SelfImprovement() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await ComposeHelper.RandomCompose(Owner, choiceContext, IsUpgraded);
        foreach (CardModel card in PileType.Draw.GetPile(Owner).Cards.Where(c => c.IsUpgradable).TakeRandom(DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardSelection))
        {
            CardCmd.Upgrade(card);
            CardCmd.Preview(card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose)
    ];
}
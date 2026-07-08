using AveMujica.AveMujicaCode.Hooks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class BlueMoon() : AveMujicaCard(4,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self), IAfterDreamspin
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone)
            return Task.CompletedTask;
        ReduceCostBy(CombatManager.Instance.History.Entries.OfType<DreamspinEntry>().Count());
        return Task.CompletedTask;
    }

    public Task AfterDreamspin(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        ReduceCostBy(1);
        return Task.CompletedTask;
    }

    public void ReduceCostBy(int amount) => EnergyCost.AddThisCombat(-amount);

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4);
    }
}
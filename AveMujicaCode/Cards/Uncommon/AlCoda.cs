using AveMujica.AveMujicaCode.Cards.CardMods;
using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class AlCoda() : AveMujicaCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose)
    ];
    
    protected override bool ShouldGlowGoldInternal => PlayedSongThisTurn;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (PlayedSongThisTurn)
        {
            await ComposeHelper.RandomCompose(Owner, choiceContext, IsUpgraded);
            await ComposeHelper.RandomCompose(Owner, choiceContext, IsUpgraded);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
    
    public bool PlayedSongThisTurn
    {
        get
        {
            return CombatManager.Instance.History.CardPlaysFinished.Any(e => e.CardPlay.Card is Song && e.HappenedThisTurn(CombatState));
        }
    }
}
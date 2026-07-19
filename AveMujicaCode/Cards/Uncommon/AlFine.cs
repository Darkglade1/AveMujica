using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Patches.Features;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class AlFine() : AveMujicaCard(1,
    CardType.Attack, CardRarity.Uncommon,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose),
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin)
    ];
    
    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.PerformsDreamspin];
    
    protected override bool ShouldGlowGoldInternal => PlayedSongThisTurn;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState).Execute(choiceContext);
            if (PlayedSongThisTurn)
            {
                await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
                await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
    
    public bool PlayedSongThisTurn
    {
        get
        {
            return CombatManager.Instance.History.CardPlaysFinished.Any(e => e.CardPlay.Card is Song && e.CardPlay.Card.Owner == Owner && e.HappenedThisTurn(CombatState));
        }
    }
}
using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class AlFine() : AveMujicaCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move), new PowerVar<Oblivion>(3)];
    
    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.GainsOblivion];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose),
        HoverTipFactory.FromPower(ModelDb.Power<Oblivion>())
    ];
    
    protected override bool ShouldGlowGoldInternal => PlayedSongThisTurn;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (PlayedSongThisTurn)
        {
            await PowerCmd.Apply<Oblivion>(choiceContext, Owner.Creature, DynamicVars["Oblivion"].BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<Oblivion>(choiceContext, Owner.Creature, DynamicVars["Oblivion"].BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Oblivion"].UpgradeValueBy(1);
    }
    
    public bool PlayedSongThisTurn
    {
        get
        {
            return CombatManager.Instance.History.CardPlaysFinished.Any(e => e.CardPlay.Card is Song && e.HappenedThisTurn(CombatState));
        }
    }
}
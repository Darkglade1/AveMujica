using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Rare;
public class Anthem() : AbstractPerformCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(3), new PowerVar<DexterityPower>(3)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform),
        HoverTipFactory.FromPower(ModelDb.Power<StrengthPower>()),
        HoverTipFactory.FromPower(ModelDb.Power<DexterityPower>())
    ];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await ExecutePerformEffect(choiceContext, play, PerformSequences()[0]);
        await ExecutePerformEffect(choiceContext, play, PerformSequences()[1]);
    }
    
    protected override List<CardType[]> PerformSequences()
    {
        CardType[] cardTypes1 = [CardType.Skill];
        CardType[] cardTypes2 = [CardType.Attack];
        return [cardTypes1, cardTypes2];
    }
    
    protected override async Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes, int numTriggers)
    {
        if (cardTypes[0] == CardType.Attack && CombatState != null)
        {
            foreach (Player player in CombatState.Players)
            {
                await PowerCmd.Apply<AnthemTempStrPower>(choiceContext, player.Creature, DynamicVars["StrengthPower"].BaseValue * numTriggers, Owner.Creature, this);
            }
        }
        if (cardTypes[0] == CardType.Skill && CombatState != null)
        {
            foreach (Player player in CombatState.Players)
            {
                await PowerCmd.Apply<AnthemTempDexPower>(choiceContext, player.Creature, DynamicVars["DexterityPower"].BaseValue * numTriggers, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
        DynamicVars["DexterityPower"].UpgradeValueBy(1);
    }
}
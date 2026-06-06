using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class Afterglow() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7, ValueProp.Move), new EnergyVar(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip, HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform)];
    
    protected override bool ShouldGlowGoldInternal => PerformedThisTurn;
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        if (PerformedThisTurn)
        {
            await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
    
    public bool PerformedThisTurn
    {
        get
        {
            return CombatManager.Instance.History.Entries.OfType<PerformCardEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner);
        }
    }
}
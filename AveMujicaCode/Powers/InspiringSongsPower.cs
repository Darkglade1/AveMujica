using AveMujica.AveMujicaCode.Cards.Token;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Powers;

public class InspiringSongsPower() : AveMujicaPower
{
    public static int EnergyIncrement = 2;
    
    public override int DisplayAmount => GetInternalData<Data>().songsPlayed % EnergyIncrement;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];
    
    protected override object InitInternalData() => new Data();
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(EnergyIncrement)];
    
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner || !(cardPlay.Card is Song))
        {
            return;
        }
        Data data = GetInternalData<Data>();
        data.songsPlayed += 1;
        int triggers = data.songsPlayed / EnergyIncrement - data.triggerCount;
        if (triggers > 0)
        {
            if (Owner.Player != null)
            {
                Flash();
                await PlayerCmd.GainEnergy( Amount * triggers, Owner.Player);
                data.triggerCount += triggers;
            }
        }
        InvokeDisplayAmountChanged();
    }
    
    public class Data
    {
        public int songsPlayed;
        public int triggerCount;
    }
    
}
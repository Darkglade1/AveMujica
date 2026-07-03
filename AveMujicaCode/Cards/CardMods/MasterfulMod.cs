using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class MasterfulMod : CardModifier
{
    private string locString;
    public MasterfulMod()
    {
        Priority = 200;
        locString = new LocString("card_mods", "AVEMUJICA-MASTERFUL-MOD.description").GetRawText();
    }
    public int BuffAmt { get; set; }
    
    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource != null && cardSource == Owner)
        {
            return !props.IsPoweredAttack() ? 0 : BuffAmt;
        }
        return 0;
    }
    
    public override Decimal ModifyBlockAdditive(
        Creature target,
        Decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource != null && cardSource == Owner)
        {
            return BuffAmt;
        }
        return 0;
    }

    public override bool ApplyStacked(CardModifier newApplied)
    {
        if (newApplied is MasterfulMod masterfulMod)
        {
            BuffAmt += masterfulMod.BuffAmt;
            return true;
        }
        return false;
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += String.Format(locString, BuffAmt);
    }
}
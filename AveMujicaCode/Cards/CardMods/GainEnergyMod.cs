using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class GainEnergyMod : CardModifier
{
    private string locString;
    public GainEnergyMod()
    {
        Priority = 125;
        locString = new LocString("card_mods", "AVEMUJICA-ENERGY-MOD.description").GetRawText();
    }
    public int GainEnergyAmt { get; set; }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await PlayerCmd.GainEnergy(GainEnergyAmt, Owner.Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += String.Format(locString, GainEnergyAmt) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
}
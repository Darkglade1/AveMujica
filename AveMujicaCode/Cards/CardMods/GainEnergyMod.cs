using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class GainEnergyMod : CardModifier
{
    public int GainEnergyAmt { get; set; }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            MainFile.Logger.Info("Hook called for " + Id);
            await PlayerCmd.GainEnergy(GainEnergyAmt, Owner.Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"Gain {GainEnergyAmt} Energy .\n";
    }
}
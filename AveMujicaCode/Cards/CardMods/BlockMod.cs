using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class BlockMod : CardModifier
{
    public int BlockAmt { get; set; }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            MainFile.Logger.Info("Hook called for " + Id);
            await CreatureCmd.GainBlock(Owner.Owner.Creature, BlockAmt, ValueProp.Unpowered, play);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"Gain {BlockAmt} Block. \n";
    }
}
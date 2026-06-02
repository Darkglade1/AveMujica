using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class ShackleMod : CardModifier
{
    public int ShackleAmt { get; set; }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
            await PowerCmd.Apply<SongTempStrPower>(choiceContext, play.Target, ShackleAmt, Owner.Owner.Creature, Owner);
        }
    }
    
    public override void AfterClonedOnCard(CardModel card)
    {
        if (card is Song song)
        {
            song.IsTargeted = true;
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"Enemy loses {ShackleAmt} Strength this turn." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
}
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class DrawCardMod : CardModifier
{
    public int DrawCardAmt { get; set; }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await CardPileCmd.Draw(choiceContext, DrawCardAmt, Owner.Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"Draw {DrawCardAmt} card(s)." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
}
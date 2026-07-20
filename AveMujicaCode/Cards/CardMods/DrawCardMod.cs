using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class DrawCardMod : CardModifier
{
    private string locString;
    private string pluralLocString;
    
    public DrawCardMod()
    {
        Priority = 150;
        locString = new LocString("card_mods", "AVEMUJICA-DRAW-CARD-MOD.description").GetRawText();
        pluralLocString = new LocString("card_mods", "AVEMUJICA-DRAW-CARD-MOD.plural_description").GetRawText();
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await CardPileCmd.Draw(choiceContext, Amount, Owner.Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        if (Amount > 1)
        {
            description += ComposeHelper.FormatedComposeString(this, String.Format(pluralLocString, Amount));
        }
        else
        {
            description += ComposeHelper.FormatedComposeString(this, String.Format(locString, Amount));
        }
    }
}
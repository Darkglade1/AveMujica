using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class SeekMod : CardModifier
{
    private string locString;
    private string pluralLocString;
    private LocString selectionScreenPrompt;
    
    public SeekMod()
    {
        Priority = 150;
        locString = new LocString("card_mods", "AVEMUJICA-SEEK-MOD.description").GetRawText();
        pluralLocString = new LocString("card_mods", "AVEMUJICA-SEEK-MOD.plural_description").GetRawText();
        selectionScreenPrompt = new LocString("card_mods", "AVEMUJICA-SEEK-MOD.selectionScreenPrompt");
    }
    public int SeekAmt { get; set; }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            CardSelectorPrefs prefs = new CardSelectorPrefs(selectionScreenPrompt, SeekAmt);
            CardModel? card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner.Owner), Owner.Owner, prefs)).FirstOrDefault();
            if (card == null)
                return;
            await CardPileCmd.Add(card, PileType.Hand);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        if (SeekAmt > 1)
        {
            description += String.Format(pluralLocString, SeekAmt) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
        }
        else
        {
            description += String.Format(locString, SeekAmt) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
        }
    }
}
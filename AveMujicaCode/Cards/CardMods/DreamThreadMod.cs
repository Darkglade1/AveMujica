using AveMujica.AveMujicaCode.Powers;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class DreamThreadMod : CardModifier
{
    private string locString;
    public DreamThreadMod()
    {
        Priority = 20;
        locString = new LocString("card_mods", "AVEMUJICA-DREAM-THREAD-MOD.description").GetRawText();
    }
    public int DreamThreadAmt { get; set; }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await PowerCmd.Apply<DreamThreadPower>(choiceContext, Owner.Owner.Creature, DreamThreadAmt, Owner.Owner.Creature, Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += String.Format(locString, DreamThreadAmt) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
}
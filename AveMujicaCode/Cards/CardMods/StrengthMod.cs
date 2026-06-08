using AveMujica.AveMujicaCode.Powers;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class StrengthMod : CardModifier
{
    private string locString;
    public StrengthMod()
    {
        Priority = 20;
        locString = new LocString("card_mods", "AVEMUJICA-STRENGTH-MOD.description").GetRawText();
    }
    public int StrengthAmt { get; set; }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Owner.Creature, StrengthAmt, Owner.Owner.Creature, Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += String.Format(locString, StrengthAmt) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
}
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class LoseStrengthMod : CardModifier
{
    private string locString;
    public LoseStrengthMod()
    {
        Priority = 20;
        locString = new LocString("card_mods", "AVEMUJICA-LOSE-STRENGTH-MOD.description").GetRawText();
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Owner.Creature, -Amount, Owner.Owner.Creature, Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += String.Format(locString, Amount) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
    
    public override void AddTips(List<IHoverTip> tips)
    {
        tips.Add(HoverTipFactory.FromPower<StrengthPower>());
    }
}
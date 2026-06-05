using AveMujica.AveMujicaCode.Powers;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class FlexMod : CardModifier
{
    public FlexMod()
    {
        Priority = 25;
    }
    public int FlexAmt { get; set; }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await PowerCmd.Apply<SongTempStrPower>(choiceContext, Owner.Owner.Creature, FlexAmt, Owner.Owner.Creature, Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"Gain {FlexAmt} [gold]Strength[/gold] this turn." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
}
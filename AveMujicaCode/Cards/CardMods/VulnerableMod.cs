using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class VulnerableMod : CardModifier
{
    private string locString;
    public VulnerableMod()
    {
        Priority = 100;
        locString = new LocString("card_mods", "AVEMUJICA-VULNERABLE-MOD.description").GetRawText();
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
            await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, Amount, Owner.Owner.Creature, Owner);
        }
    }
    
    public override void OnInitialApplication()
    {
        if (Owner is Song song)
        {
            song.IsTargeted = true;
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += String.Format(locString, Amount) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
    
    public override void AddTips(List<IHoverTip> tips)
    {
        tips.Add(HoverTipFactory.FromPower<VulnerablePower>());
    }
}
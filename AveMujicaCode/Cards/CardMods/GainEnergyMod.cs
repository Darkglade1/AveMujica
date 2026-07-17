using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class GainEnergyMod : CardModifier
{
    private string locString;
    private string energyIcon;
    public GainEnergyMod()
    {
        Priority = 125;
        locString = new LocString("card_mods", "AVEMUJICA-ENERGY-MOD.description").GetRawText();
        var characterEnergyPrefix = RunManager.Instance.GetLocalCharacterEnergyIconPrefix();
        if (characterEnergyPrefix == null)
        {
            characterEnergyPrefix = "colorless";
        }
        energyIcon = $"[img]res://images/packed/sprite_fonts/{characterEnergyPrefix}_energy_icon.png[/img]";
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await PlayerCmd.GainEnergy(Amount, Owner.Owner);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        var energyIcons = string.Concat(Enumerable.Repeat(energyIcon, Amount));
        description += String.Format(locString, energyIcons) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
    
    public override void AddTips(List<IHoverTip> tips)
    {
        if (Owner != null)
        {
            tips.Add(HoverTipFactory.ForEnergy(Owner));
        }
    }
}
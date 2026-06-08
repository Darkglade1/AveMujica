using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class BlockMod : CardModifier
{
    private string locString;
    public BlockMod()
    {
        Priority = -100;
        locString = new LocString("card_mods", "AVEMUJICA-BLOCK-MOD.description").GetRawText();
    }
    public DynamicVar? BlockVar
    {
        get;
        set;
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner && BlockVar != null)
        {
            await CreatureCmd.GainBlock(Owner.Owner.Creature, BlockVar.BaseValue, ValueProp.Move, play);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        if (BlockVar != null)
        {
            var roundedBlock = Math.Floor(BlockVar.PreviewValue);
            if (roundedBlock != BlockVar.BaseValue)
            {
                if (roundedBlock > BlockVar.BaseValue)
                {
                    var greenBlock = $"[green]{roundedBlock}[/green]";
                    description += String.Format(locString, greenBlock) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
                }
                else
                {
                    var redBlock = $"[red]{roundedBlock}[/red]";
                    description += String.Format(locString, redBlock) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
                }
            }
            else
            {
                description += String.Format(locString, BlockVar.BaseValue) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
            }
        }
    }
}
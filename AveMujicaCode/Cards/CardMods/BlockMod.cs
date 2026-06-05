using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class BlockMod : CardModifier
{
    public BlockMod()
    {
        Priority = -100;
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
            var roundedBLock = Math.Floor(BlockVar.PreviewValue);
            if (roundedBLock != BlockVar.BaseValue)
            {
                if (roundedBLock > BlockVar.BaseValue)
                {
                    description += $"Gain [green]{roundedBLock}[/green] [gold]Block[/gold]." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
                }
                else
                {
                    description += $"Gain [red]{roundedBLock}[/red] [gold]Block[/gold]." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
                }
            }
            else
            {
                description += $"Gain {BlockVar.BaseValue} [gold]Block[/gold]." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
            }
        }
    }
}
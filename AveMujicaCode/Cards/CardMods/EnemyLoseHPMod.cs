using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class EnemyLoseHPMod : CardModifier
{
    private string locString;
    public EnemyLoseHPMod()
    {
        Priority = -15;
        locString = new LocString("card_mods", "AVEMUJICA-ENEMY-LOSE-HP-MOD.description").GetRawText();
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner && play.Target != null)
        {
            await CreatureCmd.Damage(choiceContext, play.Target, Amount, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Owner.Owner.Creature);
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
}
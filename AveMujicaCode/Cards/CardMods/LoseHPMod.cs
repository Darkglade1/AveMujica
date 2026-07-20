using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class LoseHPMod : CardModifier
{
    private string locString;
    public LoseHPMod()
    {
        Priority = -5;
        locString = new LocString("card_mods", "AVEMUJICA-LOSE-HP-MOD.description").GetRawText();
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Owner.Creature, Amount, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Owner.Owner.Creature);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += ComposeHelper.FormatedComposeString(this, String.Format(locString, Amount));
    }
}
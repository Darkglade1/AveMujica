
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public class DolorisBuffIntent : BuffIntent
{
    private DolorisAlly doloris;

    public DolorisBuffIntent(DolorisAlly doloris)
    {
        this.doloris = doloris;
    }
    protected override LocString GetIntentDescription(IEnumerable<Creature> targets, Creature owner)
    {
        LocString intentDescription = new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_AUTO.description");
        // TODO: Unhardcode this shit
        //intentDescription = String.Format(intentDescription.GetRawText(), doloris.playerStrength);
        return intentDescription;
    }
}



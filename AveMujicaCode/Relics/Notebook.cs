using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Relics;

public class Notebook() : AveMujicaRelic, IOnFinishComposing
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose), HoverTipFactory.FromPower<StrengthPower>()];
    
    public async Task OnFinishComposing(Player composer, CardModel card)
    {
        if (Owner.Creature.CombatState == null || Owner != composer)
        {
            return;
        }
        Flash();
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["StrengthPower"].IntValue, Owner.Creature, null);
    }
}
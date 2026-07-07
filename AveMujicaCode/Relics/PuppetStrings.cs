using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Relics;

public class PuppetStrings() : AveMujicaRelic, IAfterAwaken
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll), HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()];
    
    public async Task AfterAwaken(PlayerChoiceContext choiceContext, Player player, AbstractDoll doll)
    {
        if (Owner.Creature.CombatState == null || Owner != player)
        {
            return;
        }
        Flash();
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), doll.Creature, DynamicVars["StrengthPower"].IntValue, doll.Creature, null);
        await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), doll.Creature, DynamicVars["StrengthPower"].IntValue, doll.Creature, null);
    }
}
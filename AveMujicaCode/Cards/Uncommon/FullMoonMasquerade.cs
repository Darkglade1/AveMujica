using AveMujica.AveMujicaCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class FullMoonMasquerade() : AveMujicaCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(11, ValueProp.Move), new PowerVar<Oblivion>(2)];
    
    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.GainsOblivion];
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        IReadOnlyList<Creature>? hittableEnemies = Owner.Creature.CombatState?.HittableEnemies;
        if (hittableEnemies != null && hittableEnemies.Count != 0)
        {
            foreach (var unused in hittableEnemies)
            {
                await PowerCmd.Apply<Oblivion>(choiceContext, Owner.Creature, DynamicVars["Oblivion"].BaseValue, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
        DynamicVars["Oblivion"].UpgradeValueBy(1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower(ModelDb.Power<Oblivion>())
    ];
}
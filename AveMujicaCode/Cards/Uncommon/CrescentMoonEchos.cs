using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class CrescentMoonEchos() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Oblivion>(4)];

    protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.GainsOblivion];
    
    protected override bool HasEnergyCostX => true;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int num = ResolveEnergyXValue();
        if (num > 0)
        {
            for (int i = 0; i < num; i++)
            {
                await PowerCmd.Apply<Oblivion>(choiceContext, Owner.Creature, DynamicVars["Oblivion"].BaseValue, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Oblivion"].UpgradeValueBy(2);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower(ModelDb.Power<Oblivion>())
    ];
}
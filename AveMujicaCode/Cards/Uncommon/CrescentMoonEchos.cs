using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class CrescentMoonEchos() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Uncommon,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DreamspinVar(2)];
    
    protected override bool HasEnergyCostX => true;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int num = ResolveEnergyXValue();
        if (num > 0)
        {
            await AllyHelper.Dreamspin(choiceContext, Owner, DynamicVars["Dreamspin"].IntValue * num, play.Target, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Dreamspin"].UpgradeValueBy(1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin)
    ];
}
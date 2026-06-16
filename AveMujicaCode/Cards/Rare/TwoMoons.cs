using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class TwoMoons() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Rare,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DreamspinVar(2)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await AllyHelper.Dreamspin(choiceContext, Owner, DynamicVars["Dreamspin"].IntValue, play.Target, this);
        if (play.Target != null && play.Target.Monster is AbstractAlly doll)
        {
            doll.numSkillsPerTurn++;
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
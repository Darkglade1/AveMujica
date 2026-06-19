using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class NewMoonAwakening() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Common,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DreamspinVar(3), new PowerVar<StrengthPower>(2)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await AllyHelper.Dreamspin(choiceContext, Owner, DynamicVars["Dreamspin"].IntValue, play.Target, this);
        if (play.Target != null && play.Target.Monster is AbstractAlly)
        {
            await PowerCmd.Apply<NewMoonTempStrPower>(choiceContext, play.Target, DynamicVars["StrengthPower"].BaseValue, Owner.Creature,this);
            await PowerCmd.Apply<NewMoonTempDexPower>(choiceContext, play.Target, DynamicVars["StrengthPower"].BaseValue, Owner.Creature,this);   
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Dreamspin"].UpgradeValueBy(1);
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin),
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll)
    ];
}
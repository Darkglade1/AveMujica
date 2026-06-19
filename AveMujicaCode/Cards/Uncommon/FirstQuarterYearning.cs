using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class FirstQuarterYearning() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    CustomTargetType.Pet)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower(ModelDb.Power<StrengthPower>()),
        HoverTipFactory.FromPower(ModelDb.Power<DexterityPower>()),
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, play.Target, DynamicVars["StrengthPower"].BaseValue, Owner.Creature,this);
            await PowerCmd.Apply<DexterityPower>(choiceContext, play.Target, DynamicVars["StrengthPower"].BaseValue, Owner.Creature,this);   
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
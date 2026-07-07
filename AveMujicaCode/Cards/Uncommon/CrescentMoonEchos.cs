using AveMujica.AveMujicaCode.Cards.Dolls;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class CrescentMoonEchos() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    CustomTargetType.PetOrSelf)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
        if (play.Target == Owner.Creature || play.IsAutoPlay)
        {
            await DollHelper.Dreamspin(choiceContext, Owner, play.Target, this);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin)
    ];
}
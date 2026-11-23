using AveMujica.AveMujicaCode.Cards.Dolls;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AveMujica.AveMujicaCode.Cards.Token;
[Pool(typeof(TokenCardPool))]
public class Amoris() : AbstractDollCard(-1,
    CardType.Power, CardRarity.Token,
    TargetType.Self)
{
    public override int StartingHp => 4;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Awaken),
        AmorisDoll.GenerateCardHoverTip()
    ];
    
    protected override bool IsSkinEnabled()
    {
        return Config.UseAmorisSkin;
    }

    public override MonsterModel GetAssociatedDoll()
    {
        return ModelDb.Monster<AmorisDoll>();
    }
}
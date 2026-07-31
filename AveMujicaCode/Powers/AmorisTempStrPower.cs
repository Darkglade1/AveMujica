using AveMujica.AveMujicaCode.Cards.Dolls;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Powers;

public class AmorisTempStrPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Monster<AmorisDoll>();
    
    public string CustomPackedIconPath => "flex.png".PowerImagePath();
    public string CustomBigIconPath => "flex.png".BigPowerImagePath();

    protected override bool IsPositive => true;
    
    public override LocString Title
    {
        get
        {
            switch (OriginModel)
            {
                case CardModel cardModel:
                    return cardModel.TitleLocString;
                case PotionModel potionModel:
                    return potionModel.Title;
                case RelicModel relicModel:
                    return relicModel.Title;
                case MonsterModel monsterModel:
                    return monsterModel.Title;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> items = new List<IHoverTip>();
            IEnumerable<IHoverTip> collection;
            switch (OriginModel)
            {
                case CardModel card:
                    collection =[HoverTipFactory.FromCard(card)];
                    break;
                case PotionModel _:
                    collection = Array.Empty<IHoverTip>();
                    break;
                case RelicModel relic:
                    collection = HoverTipFactory.FromRelic(relic);
                    break;
                case MonsterModel _:
                    collection = Array.Empty<IHoverTip>();
                    break;
                default:
                    throw new InvalidOperationException();
            }
            items.AddRange(collection);
            items.Add(HoverTipFactory.FromPower<StrengthPower>());
            return items;
        }
    }
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            Flash();
            await PowerCmd.Remove(this);
            await PowerCmd.Apply<StrengthPower>(choiceContext,Owner,-Sign * Amount,Owner,null);
        }
    }
}
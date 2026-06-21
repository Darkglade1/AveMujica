using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Powers;

public class LargeRepertoirePower : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player && Owner.Player != null)
        {
            IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(Owner.Player).Cards;
            if (cards.Count == 0)
                return;
            int amount = cards.Count * Amount;
            Flash();
            await CreatureCmd.GainBlock(Owner, amount, ValueProp.Unpowered, null);
        }
    }
}
using AveMujica.AveMujicaCode.Enchantments;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Powers;

public class VirtuosoFormPower : AveMujicaPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        foreach (var card in PileType.Hand.GetPile(player).Cards)
        {
            if (card.Type == CardType.Attack || card.GainsBlock)
            {
                CardCmd.Enchant<Masterful>(card, Amount);
            }
        }
        return Task.CompletedTask;
    }
}
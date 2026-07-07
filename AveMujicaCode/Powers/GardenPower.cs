// using AveMujica.AveMujicaCode.Cards.Allies;
// using AveMujica.AveMujicaCode.Hooks;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Players;
// using MegaCrit.Sts2.Core.Entities.Powers;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//
// namespace AveMujica.AveMujicaCode.Powers;
//
// public class GardenPower() : AveMujicaPower, IAfterDollSkill
// {
//     public override PowerType Type =>
//         PowerType.Buff;
//
//     public override PowerStackType StackType =>
//         PowerStackType.Counter;
//
//     public async Task AfterDollSkill(Player player, AbstractAlly ally)
//     {
//         if (Owner.Player != null && player == Owner.Player)
//         {
//             Flash();
//             await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), Amount, Owner.Player);
//         }
//     }
// }
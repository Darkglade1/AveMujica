// using AveMujica.AveMujicaCode.Cards.Allies;
// using MegaCrit.Sts2.Core.Combat;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.Entities.Powers;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Models.Powers;
//
// namespace AveMujica.AveMujicaCode.Powers;
//
// public class KnightsOfOblivionisPower() : AveMujicaPower
// {
//     public override PowerType Type =>
//         PowerType.Buff;
//
//     public override PowerStackType StackType =>
//         PowerStackType.Counter;
//     
//     public override async Task BeforeSideTurnStart(
//         PlayerChoiceContext choiceContext,
//         CombatSide side,
//         IReadOnlyList<Creature> participants,
//         ICombatState combatState)
//     {
//         if (side == CombatSide.Player && Owner.CombatState != null)
//         {
//             Flash();
//             foreach (var ally in Owner.CombatState.Allies)
//             {
//                 if (ally.IsPet && ally.IsAlive && ally.PetOwner == Owner.Player && ally.Monster is AbstractAlly)
//                 {
//                     await PowerCmd.Apply<StrengthPower>(choiceContext, ally, Amount, Owner,null);
//                     await PowerCmd.Apply<DexterityPower>(choiceContext, ally, Amount, Owner,null);   
//                 }
//             }
//         }
//     }
// }
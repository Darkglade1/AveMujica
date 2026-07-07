// using AveMujica.AveMujicaCode.Cards.Allies;
// using MegaCrit.Sts2.Core.Entities.Multiplayer;
// using MegaCrit.Sts2.Core.Entities.Players;
// using MegaCrit.Sts2.Core.GameActions;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Models;
//
// namespace AveMujica.AveMujicaCode.Actions;
//
// public sealed class AllyButtonAction : GameAction
// {
//   public override ulong OwnerId => Player.NetId;
//
//   public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly;
//
//   public Player Player { get; }
//
//   public int ButtonNum { get; }
//   public ModelId ModelId { get; }
//
//   public AllyButtonAction(Player player, int buttonNum, ModelId modelId)
//   {
//     Player = player;
//     ModelId = modelId;
//     ButtonNum = buttonNum;
//   }
//
//   protected override async Task ExecuteAction()
//   {
//     if (Player.Creature.CombatState == null)
//     {
//       return;
//     }
//     var creature = Player.Creature.CombatState.Allies.FirstOrDefault(c => c.ModelId == ModelId && c.PetOwner == Player && c.IsAlive);
//     if (creature?.Monster is AbstractAlly ally)
//     {
//       if (ButtonNum == 1)
//       {
//         if (ally.Creature.CurrentHp >= ally.GetSkill1HPCost() && ally.CanUseSkill())
//         {
//           await ally.Skill1();
//         }
//       }
//       else
//       {
//         if (ally.Creature.CurrentHp >= ally.GetSkill2HPCost() && ally.CanUseSkill())
//         {
//           await ally.Skill2();
//         }
//       }
//     }
//   }
//
//   public override INetAction ToNetAction()
//   {
//     return new NetAllyButtonAction()
//     {
//       modelId = ModelId,
//       buttonNum = ButtonNum
//     };
//   }
//
//   public override string ToString()
//   {
//     return $"{nameof (AllyButtonAction)}modelId: {ModelId} buttonNum: {ButtonNum}";
//   }
// }

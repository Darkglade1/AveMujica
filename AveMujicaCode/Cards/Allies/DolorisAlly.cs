using AveMujica.AveMujicaCode.Extensions;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public sealed class DolorisAlly : AbstractAlly
{
  public override string CustomVisualPath => "oblvns.tscn".CharacterPath();

  protected override MoveState GetDefaultMoveState()
  {
    return new MoveState("BUFF_MOVE", Buff, new BuffIntent());
  }
  
  private async Task Buff(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), owner.Creature, 1, Creature, null);
      await CreatureCmd.GainMaxHp(Creature, 2);
    }
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("Idle", isLooping: true);
    // AnimState animState2 = new AnimState("cast");
    // AnimState animState3 = new AnimState("attack");
    // AnimState animState4 = new AnimState("hurt");
    // AnimState state = new AnimState("die");
    // AnimState animState5 = new AnimState("shiv");
    // AnimState animState6 = new AnimState("relaxed_loop", isLooping: true);
    // animState2.NextState = animState;
    // animState3.NextState = animState;
    // animState4.NextState = animState;
    // animState5.NextState = animState;
    // animState6.AddBranch("Idle", animState);
    CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
    creatureAnimator.AddAnyState("Idle", animState);
    // creatureAnimator.AddAnyState("Dead", state);
    // creatureAnimator.AddAnyState("Hit", animState4);
    // creatureAnimator.AddAnyState("Attack", animState3);
    // creatureAnimator.AddAnyState("Cast", animState2);
    // creatureAnimator.AddAnyState("Shiv", animState5);
    // creatureAnimator.AddAnyState("Relaxed", animState6);
    return creatureAnimator;
  }
}

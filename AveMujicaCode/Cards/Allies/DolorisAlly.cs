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
  public override string CustomVisualPath => "doloris/doloris.tscn".CharacterPath();
  
  protected override MoveState GetDefaultMoveState()
  {
    return new MoveState("BUFF_MOVE", Buff, new BuffIntent());
  }
  
  private async Task Buff(IReadOnlyList<Creature> targets)
  {
    var owner = Creature.PetOwner;
    if (owner != null)
    {
      await CreatureCmd.TriggerAnim(Creature, "Cast", 0);
      await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), owner.Creature, 1, Creature, null);
      await CreatureCmd.GainMaxHp(Creature, 2);
    }
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState startState = new AnimState("Start");
    AnimState animState = new AnimState("Idle", isLooping: true);
    AnimState animState2 = new AnimState("Skill_1_Begin");
    AnimState animState3 = new AnimState("Skill_2_Begin");
    AnimState animState4 = new AnimState("Skill_2_Loop");
    AnimState animState5 = new AnimState("Skill_2_End");
    AnimState state = new AnimState("Die");
    startState.NextState = animState;
    animState2.NextState = animState;
    animState3.NextState = animState4;
    animState4.NextState = animState5;
    animState5.NextState = animState;
    CreatureAnimator creatureAnimator = new CreatureAnimator(startState, controller);
    creatureAnimator.AddAnyState("Idle", animState);
    creatureAnimator.AddAnyState("Dead", state);
    creatureAnimator.AddAnyState("Attack", animState3);
    creatureAnimator.AddAnyState("Cast", animState2);
    return creatureAnimator;
  }
}

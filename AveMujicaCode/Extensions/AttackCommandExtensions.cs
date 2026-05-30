using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Models;

public static class AttackCommandExtensions
{
    public static AttackCommand FromPet(this AttackCommand command, MonsterModel pet)
    {
        command.Attacker = command.Attacker == null
            ? pet.Creature
            : throw new InvalidOperationException("Attacker has already been set.");
        command._attackerAnimName = "Attack";
        command._sourceType = AttackCommand.SourceType.None;
        return command;
    }
}
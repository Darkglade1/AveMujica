using AveMujica.AveMujicaCode.Cards.Allies;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

public class AllyHelper
{
    public static async Task<Creature> Awaken<T>(
        PlayerChoiceContext ctx,
        Player summoner,
        int hp,
        AbstractModel? source) where T : MonsterModel
    {
        var combatState = summoner.Creature.CombatState;
        ArgumentNullException.ThrowIfNull(combatState);
        ArgumentNullException.ThrowIfNull(summoner.PlayerCombatState);
        var existing = combatState.Allies.FirstOrDefault(c => c.Monster is T && c.PetOwner == summoner && c.IsAlive);
    
        if (existing != null)
        {
            await CreatureCmd.GainMaxHp(existing, hp);
        }
        else
        {
            existing = await PlayerCmd.AddPet<T>(summoner);
            var playerNode = NCombatRoom.Instance?.GetCreatureNode(summoner.Creature);
            var index = 0;
            foreach (Creature pet in summoner.Creature.Pets)
            {
                if (pet.Monster is AbstractAlly && pet.IsAlive)
                {
                    var node = NCombatRoom.Instance?.GetCreatureNode(pet);
                    if (node != null && source is CardModel && playerNode != null)
                    {
                        node.Position = CalculatePosition(index);
                        node.Modulate = Colors.Transparent;
                        node.CreateTween()
                            .TweenProperty(node, "modulate", Colors.White, 0.35)
                            .SetDelay(0.1);
                    }
                    node?.TrackBlockStatus(summoner.Creature);
                    node?.ToggleIsInteractable(true);
                    index++;
                }
            }
            await CreatureCmd.SetMaxHp(existing, hp);
            await CreatureCmd.Heal(existing, hp, false);
        }
        
        return existing;
    }

    public static Vector2 CalculatePosition(int index)
    {
        var firstPosition = new Vector2(-230f, 125f);
        var secondPosition = new Vector2(-630f, 125f);
        var thirdPosition = new Vector2(-780f, 125f);
        var fourthPosition = new Vector2(-80f, 125f);
        switch (index)
        {
            case 0:
                return firstPosition;
            case 1:
                return secondPosition;
            case 2:
                return thirdPosition;
            default:
                return fourthPosition;
        }
    }
}


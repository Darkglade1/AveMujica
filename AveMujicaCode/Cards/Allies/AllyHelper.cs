using AveMujica.AveMujicaCode;
using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Cards.Uncommon;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

public class AllyHelper
{
    public static async Task Awaken<T>(
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
                        node.Position = CalculatePosition(index) + playerNode.Position;
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
    }

    public static Vector2 CalculatePosition(int index)
    {
        var firstPosition = new Vector2(250f, -75f);
        var secondPosition = new Vector2(-150f, -75f);
        var thirdPosition = new Vector2(-320f, -75f);
        var fourthPosition = new Vector2(420f, -75f);
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

    public static List<CardModel> AllyCardList()
    {
        var cardList = new List<CardModel>();
        cardList.Add((CardModel)ModelDb.Card<Doloris>().MutableClone());
        cardList.Add((CardModel)ModelDb.Card<Mortis>().MutableClone());
        cardList.Add((CardModel)ModelDb.Card<Timoris>().MutableClone());
        cardList.Add((CardModel)ModelDb.Card<Amoris>().MutableClone());
        return cardList;
    }
}


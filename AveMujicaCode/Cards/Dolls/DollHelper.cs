using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AveMujica.AveMujicaCode.Cards.Dolls;

public class DollHelper
{
    public static int StartingHp = 4;
    public static async Task Awaken<T>(
        PlayerChoiceContext choiceContext,
        Player summoner,
        int hp) where T : MonsterModel
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
                if (pet.Monster is AbstractDoll && pet.IsAlive)
                {
                    var node = NCombatRoom.Instance?.GetCreatureNode(pet);
                    if (node != null && playerNode != null)
                    {
                        node.Position = CalculatePosition(index) + playerNode.Position;
                        node.Modulate = Colors.Transparent;
                        node.CreateTween()
                            .TweenProperty(node, "modulate", Colors.White, 0.35)
                            .SetDelay(0.1);
                    }
                    node?.ToggleIsInteractable(true);
                    index++;
                }
            }
            await CreatureCmd.SetMaxHp(existing, hp);
            await CreatureCmd.Heal(existing, hp, false);
            if (existing.Monster is AbstractDoll ally)
            {
                await AveMujicaHooks.AfterAwaken(summoner.RunState, summoner.Creature.CombatState, choiceContext, summoner, ally);
            }
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

    public static async Task Dreamspin(PlayerChoiceContext choiceContext, Player player, Creature? target, CardModel? sourceCard)
    {
        if (target != null && target.IsPet && target.Monster is AbstractDoll ally)
        {
            await ally.Skill();
        }
        else
        {
            if (player.Creature.CombatState == null)
            {
                return;
            }
            var doloris = (CardModel)ModelDb.Card<Doloris>().MutableClone();
            var mortis = (CardModel)ModelDb.Card<Mortis>().MutableClone();
            var timoris = (CardModel)ModelDb.Card<Timoris>().MutableClone();
            var amoris = (CardModel)ModelDb.Card<Amoris>().MutableClone();
            List<CardModel> dolls =
            [
                doloris,
                mortis,
                timoris,
                amoris
            ];
            foreach (CardModel card in dolls)
            {
                if (player.Creature.CombatState != null)
                {
                    player.Creature.CombatState.AddCard(card, player);
                }
            }

            if (player.Creature.CombatState != null)
            {
                foreach (var creature in player.Creature.CombatState.Allies)
                {
                    if (creature.IsPet && creature.PetOwner == player && creature.IsAlive)
                    {
                        if (creature.Monster is DolorisDoll)
                        {
                            dolls.Remove(doloris);
                        }

                        if (creature.Monster is MortisDoll)
                        {
                            dolls.Remove(mortis);
                        }

                        if (creature.Monster is TimorisDoll)
                        {
                            dolls.Remove(timoris);
                        }

                        if (creature.Monster is AmorisDoll)
                        {
                            dolls.Remove(amoris);
                        }
                    }
                }

                if (dolls.Count > 0)
                {
                    dolls.StableShuffle(player.RunState.Rng.CombatCardGeneration);
                    while (dolls.Count > 3)
                    {
                        dolls.RemoveAt(dolls.Count - 1);
                    }

                    var pickedCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, dolls, player);
                    if (pickedCard != null)
                    {
                        if (pickedCard is Doloris)
                        {
                            await Awaken<DolorisDoll>(choiceContext, player, StartingHp);
                        }
                        if (pickedCard is Mortis)
                        {
                            await Awaken<MortisDoll>(choiceContext, player, StartingHp);
                        }
                        if (pickedCard is Timoris)
                        {
                            await Awaken<TimorisDoll>(choiceContext, player, StartingHp);
                        }
                        if (pickedCard is Amoris)
                        {
                            await Awaken<AmorisDoll>(choiceContext, player, StartingHp);
                        }
                    }
                }
                else
                {
                    foreach (var doll in player.Creature.CombatState.Allies)
                    {
                        if (doll.Monster is AbstractDoll && doll.PetOwner == player && doll.IsAlive)
                        {
                            await CreatureCmd.GainMaxHp(doll, 1);
                        }
                    }
                }
            }
        }
        await AveMujicaHooks.AfterDreamspin(player.RunState, player.Creature.CombatState, choiceContext, player);
        ICombatState? combatState = player.Creature.CombatState;
        if (!CombatManager.Instance.IsOverOrEnding && combatState != null)
        {
            CombatManager.Instance.History.Add(combatState, new DreamspinEntry(player.Creature, combatState.RoundNumber, combatState.CurrentSide, CombatManager.Instance.History, combatState.Players));
        }
    }
}
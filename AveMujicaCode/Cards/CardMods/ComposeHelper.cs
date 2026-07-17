using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Hooks;
using AveMujica.AveMujicaCode.Overlays;
using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class ComposeHelper
{
    public class ComposeFields {
        public static readonly SpireField<ICombatState, Dictionary<Player, int>> CurrentComposeNum = new(() => new Dictionary<Player, int>());
        public static readonly SpireField<ICombatState, Dictionary<Player, CardModel?>> CurrentSong = new(() => new Dictionary<Player, CardModel?>());
    }

    public static int NumComposesSongComplete = 3;
    public class WeightedComposeEffect
    {
        public required CardModifier ComposeEffect { get; set; }
        public int Weight { get; set; }
    }
    
    public static async Task RandomCompose(Player owner, PlayerChoiceContext choiceContext, bool isUpgraded)
    {
        var composeEffects = GenerateRandomComposeEffects(owner, isUpgraded);
        var cardsToChoose = new CardModel[composeEffects.Count];
        for (int i = 0; i < composeEffects.Count; i++)
        {
            CardModel song = (CardModel)ModelDb.Card<Song>().MutableClone();
            CardModifier.AddModifier(song, composeEffects[i]);
            cardsToChoose[i] = song;
        }
        var cardsList = cardsToChoose.ToList();

        foreach (var c in cardsList)
        {
            c.Owner = owner;
        }

        var card = await CardSelectCmd.FromChooseACardScreen(
            choiceContext,
            cardsToChoose,
            owner
        );

        if (card != null)
        {
            await AddComposeEffectsToSong(CardModifier.Modifiers(card), card.Owner);
        }
    }

    public static async Task AddComposeEffectsToSong(IReadOnlyCollection<CardModifier> mods, Player owner)
    {
        var combatState = owner.Creature.CombatState;
        if (combatState != null)
        {
            var songDict = ComposeFields.CurrentSong.Get(combatState);
            if (songDict == null)
            {
                songDict = new Dictionary<Player, CardModel?>();
            }
            CardModel? currentSong = songDict.GetValueOrDefault(owner);
            if (currentSong == null)
            {
                Song newSong = combatState.CreateCard<Song>(owner);
                currentSong = newSong;
            }

            foreach (var cardMod in mods)
            {
                CardModifier.AddModifier(currentSong, cardMod);
            }

            var numComposeDict = ComposeFields.CurrentComposeNum.Get(combatState);
            if (numComposeDict == null)
            {
                numComposeDict = new Dictionary<Player, int>();
            }

            var numComposes = numComposeDict.GetValueOrDefault(owner);
            numComposes++;
            if (numComposes >= NumComposesSongComplete)
            {
                await CardPileCmd.AddGeneratedCardToCombat(currentSong, PileType.Hand, owner);
                await AveMujicaHooks.OnFinishComposing(owner.RunState, owner.Creature.CombatState, owner, currentSong);
                songDict[owner] = null;
                numComposeDict[owner] = 0;
            }
            else
            {
                songDict[owner] = currentSong;
                numComposeDict[owner] = numComposes;
            }
            ComposeFields.CurrentSong.Set(combatState, songDict);
            ComposeFields.CurrentComposeNum.Set(combatState, numComposeDict);
            SongPreview.Instance?.UpdateDisplay(combatState, owner);
        }
    }

    private static List<CardModifier> GenerateRandomComposeEffects(Player owner, bool isUpgraded)
    {
        var damageMod = (DamageMod)ModelDb.Get<DamageMod>().MutableClone();
        damageMod.Amount = isUpgraded ? 8 : 6;
        var blockMod = (BlockMod)ModelDb.Get<BlockMod>().MutableClone();
        blockMod.Amount = isUpgraded ? 6 : 4;
        var shackleMod = (ShackleMod)ModelDb.Get<ShackleMod>().MutableClone();
        shackleMod.Amount = isUpgraded ? 4 : 3;
        var weakMod = (WeakMod)ModelDb.Get<WeakMod>().MutableClone();
        weakMod.Amount = isUpgraded ? 3 : 2;
        var vulnerableMod = (VulnerableMod)ModelDb.Get<VulnerableMod>().MutableClone();
        vulnerableMod.Amount = isUpgraded ? 3 : 2;
        var drawCardMod = (DrawCardMod)ModelDb.Get<DrawCardMod>().MutableClone();
        drawCardMod.Amount = isUpgraded ? 2 : 1;
        var gainEnergyMod = (GainEnergyMod)ModelDb.Get<GainEnergyMod>().MutableClone();
        gainEnergyMod.Amount = 1;
        
        var items = new List<WeightedComposeEffect>
        {
            new WeightedComposeEffect { ComposeEffect = damageMod, Weight = 35 },
            new WeightedComposeEffect { ComposeEffect = blockMod, Weight = 25 },
            new WeightedComposeEffect { ComposeEffect = shackleMod, Weight = 10 },
            new WeightedComposeEffect { ComposeEffect = weakMod, Weight = 10 },
            new WeightedComposeEffect { ComposeEffect = vulnerableMod, Weight = 10 },
            new WeightedComposeEffect { ComposeEffect = drawCardMod, Weight = 5 },
            new WeightedComposeEffect { ComposeEffect = gainEnergyMod, Weight = 5 }
        };

        var returnedItems = GetWeightedRandom(items, owner,3);
        var returnedComposeEffects = new List<CardModifier>();
        foreach (var item in returnedItems)
        {
            returnedComposeEffects.Add(item.ComposeEffect);
        }

        return returnedComposeEffects;
    }
    
    public static List<WeightedComposeEffect> GetWeightedRandom(List<WeightedComposeEffect> items, Player owner, int amount)
    {
        var returnedItems = new List<WeightedComposeEffect>();
        for (int num = 0; num < amount; num++)
        {
            foreach (var item in returnedItems)
            {
                items.Remove(item);
            }
            // 1. Calculate total weight
            int totalWeight = items.Sum(i => i.Weight);

            // 2. Pick a random number between 0 and totalWeight
            int randomRoll = owner.RunState.Rng.CombatCardGeneration.NextInt(0, totalWeight);

            // 3. Iterate and subtract weight to find the item
            foreach (var item in items)
            {
                if (randomRoll < item.Weight)
                {
                    returnedItems.Add(item);
                    break;
                }
                randomRoll -= item.Weight;
            }
        }
        return returnedItems;
    }

    public static string GetNewLineIfNotLastCardMod(CardModifier cardMod)
    {
        if (cardMod.Owner == null)
        {
            return "";
        }
        var allMods = CardModifier.Modifiers(cardMod.Owner);
        if (allMods.Last() == cardMod)
        {
            return "";
        }
        else
        {
            return "\n";
        }
    }
    
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.UpdateDynamicVarPreview))]
    public static class UpdateCardModPreview
    {
        public static void Postfix(CardModel __instance, CardPreviewMode previewMode,
            Creature? target,
            DynamicVarSet dynamicVarSet)
        {
            if (__instance is Song)
            {
                var cardMods = CardModifier.Modifiers(__instance);
                foreach (var cardMod in cardMods)
                {
                    if (__instance.CombatState != null)
                    {
                        if (cardMod is DamageMod damageMod)
                        {
                            damageMod.DamageVar?.UpdateCardPreview(__instance, previewMode, target, true);
                        }
                        if (cardMod is BlockMod blockMod)
                        {
                            blockMod.BlockVar?.UpdateCardPreview(__instance, previewMode, target, true);
                        }
                    }
                }
            }
        }
    }
}
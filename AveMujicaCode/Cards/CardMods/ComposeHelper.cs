using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class ComposeHelper
{
    public class ComposeFields {
        public static readonly SpireField<Player, int> CurrentComposeNum = new(() => 0);
        public static readonly SpireField<Player, CardModel> CurrentSong = new(() => null);
    }

    public static int NumComposesSongComplete = 3;
    public class WeightedComposeEffect
    {
        public required CardModifier ComposeEffect { get; set; }
        public int Weight { get; set; }
    }
    
    public static async Task RandomCompose(Player owner, PlayerChoiceContext choiceContext, bool isUpgraded)
    {
        var composeEffects = GenerateRandomComposeEffects(isUpgraded);
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

        if (card is Song selectedSong) await selectedSong.OnSongSelected(CardModifier.Modifiers(card));
    }

    private static List<CardModifier> GenerateRandomComposeEffects(bool isUpgraded)
    {
        var damageMod = (DamageMod)ModelDb.Get<DamageMod>().MutableClone();
        damageMod.DamageAmt = isUpgraded ? 8 : 6;
        var blockMod = (BlockMod)ModelDb.Get<BlockMod>().MutableClone();
        blockMod.BlockAmt = isUpgraded ? 6 : 4;
        var randomDamageMod = (RandomDamageMod)ModelDb.Get<RandomDamageMod>().MutableClone();
        randomDamageMod.DamageAmt = isUpgraded ? 10 : 8;
        var shackleMod = (ShackleMod)ModelDb.Get<ShackleMod>().MutableClone();
        shackleMod.ShackleAmt = isUpgraded ? 4 : 3;
        var weakMod = (WeakMod)ModelDb.Get<WeakMod>().MutableClone();
        weakMod.WeakAmt = isUpgraded ? 3 : 2;
        var vulnerableMod = (VulnerableMod)ModelDb.Get<VulnerableMod>().MutableClone();
        vulnerableMod.VulnerableAmt = isUpgraded ? 3 : 2;
        var drawCardMod = (DrawCardMod)ModelDb.Get<DrawCardMod>().MutableClone();
        drawCardMod.DrawCardAmt = isUpgraded ? 2 : 1;
        var gainEnergyMod = (GainEnergyMod)ModelDb.Get<GainEnergyMod>().MutableClone();
        gainEnergyMod.GainEnergyAmt = isUpgraded ? 2 : 1;
        
        var items = new List<WeightedComposeEffect>
        {
            new WeightedComposeEffect { ComposeEffect = damageMod, Weight = 25 },
            new WeightedComposeEffect { ComposeEffect = blockMod, Weight = 25 },
            new WeightedComposeEffect { ComposeEffect = randomDamageMod, Weight = 10 },
            new WeightedComposeEffect { ComposeEffect = shackleMod, Weight = 10 },
            new WeightedComposeEffect { ComposeEffect = weakMod, Weight = 10 },
            new WeightedComposeEffect { ComposeEffect = vulnerableMod, Weight = 10 },
            new WeightedComposeEffect { ComposeEffect = drawCardMod, Weight = 5 },
            new WeightedComposeEffect { ComposeEffect = gainEnergyMod, Weight = 5 }
        };

        var returnedItems = GetWeightedRandom(items, 3);
        var returnedComposeEffects = new List<CardModifier>();
        foreach (var item in returnedItems)
        {
            returnedComposeEffects.Add(item.ComposeEffect);
        }

        return returnedComposeEffects;
    }
    
    public static List<WeightedComposeEffect> GetWeightedRandom(List<WeightedComposeEffect> items, int amount)
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
            Random rng = new Random();
            int randomRoll = rng.Next(0, totalWeight);

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
}
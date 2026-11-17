using AveMujica.AveMujicaCode.Cards.CardMods;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;

namespace AveMujica.AveMujicaCode.Patches;

[HarmonyPatch(typeof (NCardHolder), "HoverScale", MethodType.Getter)]
public static class CardHolderPatchHoverScale
{
    private static bool Prefix(NCardHolder __instance, ref Vector2 __result)
    {
        if (__instance.CardModel != null && __instance.CardModel.Owner.Creature.CombatState != null)
        {
            var songDict = ComposeHelper.ComposeFields.CurrentSong.Get(__instance.CardModel.Owner.Creature.CombatState);
            if (songDict != null)
            {
                var currentSong = songDict[__instance.CardModel.Owner];
                if (__instance.CardModel == currentSong)
                {
                    __result = Vector2.One * 1.28f;
                    return false;
                }
            } 
        }
        return true;
    }
}
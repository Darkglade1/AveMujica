using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Overlays;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public abstract class AllyCard(int cost, CardType type, CardRarity rarity, TargetType target)
    : AveMujicaCard(cost, type, rarity, target)
{
    
}

[HarmonyPatch(typeof(NCard), nameof(NCard.Reload))]
public static class FullArtAllyCards
{
    public static void Postfix(NCard __instance)
    {
        if (__instance.Model is AllyCard && __instance.IsNodeReady())
        {
            __instance._ancientPortrait.Visible = true;
            __instance._ancientBorderGlassOverlay.Visible = true;
            __instance._ancientBorder.Visible = true;
            __instance._ancientTextBg.Visible = true;
            if (__instance._canvasGroupMaskMaterial == null)
                __instance._canvasGroupMaskMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_mask_material.tres");
            __instance._portraitCanvasGroup.Material = __instance._canvasGroupMaskMaterial;
            __instance._ancientBorder.Texture = __instance.Model.AncientBorder;
            __instance._ancientTextBg.Texture = ResourceLoader.Load<Texture2D>(ImageHelper.GetImagePath($"atlases/compressed_atlas.sprites/ancient_text_bg_{__instance.Model.Type.ToString().ToLowerInvariant()}.png.tres"));
            __instance._ancientPortrait.Texture = __instance.Model.Portrait;
            __instance._banner.Material = null;
            __instance.ReloadOverlay();
            __instance._frame.Visible = false;
            __instance._ancientBorder.Visible = true;
        }
    }
}
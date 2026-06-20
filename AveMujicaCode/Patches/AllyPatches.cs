using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Cards.Token;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(AbstractIntent), nameof(AbstractIntent.GetHoverTip))]
public static class SetAllyIntentHoverTip
{
    public static void Postfix(AbstractIntent __instance, IEnumerable<Creature> targets, Creature owner, ref HoverTip __result)
    {
        if (owner.Monster is AbstractAlly ally)
        {
            __result = ally.GetInCombatAutoSkillHoverTip();
        }
    }
}

[HarmonyPatch(typeof(AttackIntent), nameof(AttackIntent.GetSingleDamage))]
public static class AlterAllyAttackIntentPreview
{
    public static void Postfix(AttackIntent __instance, IEnumerable<Creature> targets, Creature owner, ref int __result)
    {
        if (owner.Monster is AbstractAlly)
        {
            if (__instance.DamageCalc != null)
            {
                var damage = __instance.DamageCalc();
                var strengthPower = owner.GetPower<StrengthPower>();
                if (strengthPower != null)
                {
                    damage += strengthPower.Amount;
                }
                __result = (int)damage;
            }
        }
    }
}

[HarmonyPatch(typeof(NCard), nameof(NCard.Reload))]
public static class FullArtAllyCards
{
    public static void Postfix(NCard __instance)
    {
        if (__instance.Model is AllyCard && __instance.IsNodeReady())
        {
            __instance._portraitBorder.Visible = false;
            __instance._portrait.Visible = false;
            __instance._frame.Visible = false;
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
            __instance.ReloadOverlay();
            __instance._ancientBorder.Visible = true;
            __instance._frame.Texture = null;
            //__instance._banner.Material = null;
        }
    }
}

[HarmonyPatch(typeof(PersonalHivePower), nameof(PersonalHivePower.AfterDamageReceived))]
public static class PatchEntomancer
{
    public static bool Prefix(PersonalHivePower __instance, PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult _,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        ref Task __result)
    {
        if (dealer != null && dealer.Monster is AbstractAlly)
        {
            __result = Task.CompletedTask;
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(ThornsPower), nameof(ThornsPower.BeforeDamageReceived))]
public static class PatchThorns
{
    public static bool Prefix(ThornsPower __instance, PlayerChoiceContext choiceContext,
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        ref Task __result)
    {
        if (dealer != null && dealer.Monster is AbstractAlly)
        {
            __result = Task.CompletedTask;
            return false;
        }
        return true;
    }
}
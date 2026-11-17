using System.Reflection;
using AveMujica.AveMujicaCode.Data;
using BaseLib.Config;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace AveMujica.AveMujicaCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "AveMujica"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);

        Harmony harmony = new(ModId);

        harmony.PatchAll(assembly);
        
        CustomLocTableManager.Register("card_mods");
        ModConfigRegistry.Register(ModId, new Config());
        
        ModManager.OnMetricsUpload += AveMujicaMetrics.OnMetricsUpload;
    }
    
    public static string GetVersion()
    {
        var mod = ModManager.GetLoadedMods().FirstOrDefault(m => m.manifest?.id == ModId);

        return mod?.manifest?.version ?? "unknown";
    }
}
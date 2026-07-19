using BaseLib.Config;

namespace AveMujica.AveMujicaCode;

public class Config: SimpleModConfig
{
    [ConfigSection("Skins")]
    public static bool UseOblivionisSkin { get; set; } = false;
    public static bool UseDolorisSkin { get; set; } = false;
    public static bool UseMortisSkin { get; set; } = false;
    public static bool UseTimorisSkin { get; set; } = false;
    public static bool UseAmorisSkin { get; set; } = false;
    
    [ConfigSection("Sound")]
    public static bool DisableAttackSoundEffect { get; set; } = false;
    
    [ConfigSection("Tutorial")]
    public static bool ViewDreamspinFtue { get; set; } = true;
}
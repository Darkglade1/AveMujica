using BaseLib.Config;

namespace AveMujica.AveMujicaCode;

public class Config: SimpleModConfig
{
    [ConfigSection("Misc")]
    public static bool UseOblivionisSkin { get; set; } = false;
    public static bool UseDolorisSkin { get; set; } = false;
    public static bool UseMortisSkin { get; set; } = false;
    public static bool UseTimorisSkin { get; set; } = false;
    public static bool UseAmorisSkin { get; set; } = false;
}
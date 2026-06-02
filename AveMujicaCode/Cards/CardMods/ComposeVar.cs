using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class ComposeVar(string name, int compose) : DynamicVar(name, compose)
{
    public const string defaultName = "Compose";

    public ComposeVar(int compose)
        : this(defaultName, compose)
    {
    }
}
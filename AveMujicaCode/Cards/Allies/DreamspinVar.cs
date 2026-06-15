using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Allies;

public class DreamspinVar(string name, int dreamspin) : DynamicVar(name, dreamspin)
{
    public const string defaultName = "Dreamspin";

    public DreamspinVar(int dreamspin)
        : this(defaultName, dreamspin)
    {
    }
}
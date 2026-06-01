using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace AveMujica.AveMujicaCode.Cards;

public static class AveMujicaKeywords
{
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Rhythm;
    
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Compose;
    
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Awaken;
}
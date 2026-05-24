using BaseLib.Abstracts;
using BaseLib.Utils;
using AveMujica.AveMujicaCode.Character;

namespace AveMujica.AveMujicaCode.Potions;

[Pool(typeof(AveMujicaPotionPool))]
public abstract class AveMujicaPotion : CustomPotionModel;
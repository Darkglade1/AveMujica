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
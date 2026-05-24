using BaseLib.Abstracts;
using AveMujica.AveMujicaCode.Extensions;
using Godot;

namespace AveMujica.AveMujicaCode.Character;

public class AveMujicaPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => AveMujica.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
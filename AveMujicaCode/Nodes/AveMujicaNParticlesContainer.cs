using Godot;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace AveMujica.AveMujicaCode.Nodes;

[GlobalClass]
public partial class AveMujicaNParticlesContainer : NParticlesContainer
{
    public override void _Ready()
    {
        base._Ready();
        if (_particles != null && _particles.Count != 0) return;
        _particles = [];
        foreach (var child in GetChildren())
            if (child is GpuParticles2D particles)
                _particles.Add(particles);
    }
}
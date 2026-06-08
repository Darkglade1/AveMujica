using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Random;

namespace AveMujica.AveMujicaCode.Vfx;

[GlobalClass]
public partial class AveMujicaNRestSiteCharacter : NRestSiteCharacter
{
    public override void _Ready()
    {
        base._Ready();
        foreach (GodotObject childSpineNode in GetChildSpineNodes())
            this.RunWhenSpineReady(new MegaSprite((Variant) childSpineNode), (Action<MegaAnimationState>) (animState =>
            {
                MegaTrackEntry? megaTrackEntry = animState.SetAnimation("Sit");
                megaTrackEntry?.SetTrackTime(megaTrackEntry.GetAnimationEnd() * Rng.Chaotic.NextFloat());
            }));
        //var isFlipped = _characterIndex % 2 == 1;
        //var node = GetNode<TextureRect>("SpineSprite");
        //if (isFlipped) node.FlipH = true;
    }
}

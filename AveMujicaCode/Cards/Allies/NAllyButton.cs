using Godot;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace AveMujica.AveMujicaCode.Cards.Allies;

[GlobalClass]
public partial class NAllyButton : BaseButton
{
    
    private TextureRect? _icon;
    private IHoverTip? _hoverTip;
    public required AbstractAlly owner;
    public int skillNum;
    
    public static readonly IHoverTip SKILL_1 = new HoverTip(
        new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.title"),
        new LocString("static_hover_tips", "AVEMUJICA-DOLORIS_ALLY_SKILL_1.description")
    );

    public override void _Ready()
    {
        var marginContainer = GetNodeOrNull<MarginContainer>("MarginContainer");
        _icon = GetNodeOrNull<TextureRect>("ButtonVisual"); 

        if (marginContainer == null)
        {
             return;
        }

        marginContainer.MouseFilter = MouseFilterEnum.Ignore;
        if (_icon != null) _icon.MouseFilter = MouseFilterEnum.Ignore;
        
        marginContainer.SetAnchorsPreset(LayoutPreset.FullRect);

        _hoverTip = SKILL_1;

        MouseFilter = MouseFilterEnum.Pass;
        Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
        Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));
        Connect(BaseButton.SignalName.Pressed, Callable.From(Skill));
    }
    
    private async void Skill()
    {
        NHoverTipSet.Remove(this);
        if (skillNum == 1)
        {
            await owner.Skill1();
        }
        else
        {
            await owner.Skill2();
        }
    }

    private void OnHovered()
    {
        if (_hoverTip != null)
        {
            NHoverTipSet? nHoverTipSet = NHoverTipSet.CreateAndShow(this, _hoverTip);
            Vector2 tooltipOffset = new Vector2(60f, -25f);
            if (nHoverTipSet != null)
            {
                nHoverTipSet.GlobalPosition = GlobalPosition + tooltipOffset;
                nHoverTipSet.MouseFilter = MouseFilterEnum.Ignore;
            }
        }
    }

    private void OnUnhovered()
    {
        NHoverTipSet.Remove(this);
    }
}
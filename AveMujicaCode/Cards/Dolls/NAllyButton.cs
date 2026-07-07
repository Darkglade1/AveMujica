// using AveMujica.AveMujicaCode.Actions;
// using Godot;
// using MegaCrit.Sts2.Core.Combat;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaCrit.Sts2.Core.Nodes.HoverTips;
// using MegaCrit.Sts2.Core.Runs;
//
// namespace AveMujica.AveMujicaCode.Cards.Allies;
//
// [GlobalClass]
// public partial class NAllyButton : BaseButton
// {
//     
//     private TextureRect? _icon;
//     private IHoverTip? _hoverTip;
//     public required AbstractAlly owner;
//     public int skillNum;
//
//     public override void _Ready()
//     {
//         var marginContainer = GetNodeOrNull<MarginContainer>("MarginContainer");
//         _icon = GetNodeOrNull<TextureRect>("ButtonVisual"); 
//
//         if (marginContainer == null)
//         {
//              return;
//         }
//
//         marginContainer.MouseFilter = MouseFilterEnum.Ignore;
//         if (_icon != null) _icon.MouseFilter = MouseFilterEnum.Ignore;
//         
//         marginContainer.SetAnchorsPreset(LayoutPreset.FullRect);
//
//         MouseFilter = MouseFilterEnum.Pass;
//         Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
//         Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));
//         Connect(BaseButton.SignalName.Pressed, Callable.From(Skill));
//     }
//
//     public override void _Process(double delta)
//     {
//         var allyHp = owner.Creature.CurrentHp;
//         int skillCost;
//         if (skillNum == 1)
//         {
//             skillCost = owner.GetSkill1HPCost();
//         }
//         else
//         {
//             skillCost = owner.GetSkill2HPCost();
//         }
//         Disabled = skillCost > allyHp || !owner.CanUseSkill();
//
//         if (_icon != null)
//         {
//             if (Disabled)
//             {
//                 _icon.Modulate = Color.Color8(128, 128, 128);
//             } 
//             else if (IsHovered())
//             {
//                 _icon.Modulate = Color.Color8(218, 175, 38);
//             }
//             else
//             {
//                 _icon.Modulate = Color.Color8(255, 255, 255);
//             }
//         }
//     }
//
//     private void Skill()
//     {
//         NHoverTipSet.Remove(this);
//         if (owner.Creature.PetOwner != null)
//         {
//             RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new AllyButtonAction(owner.Creature.PetOwner, skillNum, owner.Creature.ModelId));
//         }
//     }
//
//     private void OnHovered()
//     {
//         if (skillNum == 1)
//         {
//             _hoverTip = owner.GetSkill1HoverTip(); 
//         }
//         else
//         {
//             _hoverTip = owner.GetSkill2HoverTip();
//         }
//         if (_hoverTip != null)
//         {
//             NHoverTipSet? nHoverTipSet = NHoverTipSet.CreateAndShow(this, _hoverTip);
//             Vector2 tooltipOffset = new Vector2(60f, -25f);
//             if (nHoverTipSet != null)
//             {
//                 nHoverTipSet.GlobalPosition = GlobalPosition + tooltipOffset;
//                 nHoverTipSet.MouseFilter = MouseFilterEnum.Ignore;
//             }
//         }
//     }
//
//     private void OnUnhovered()
//     {
//         NHoverTipSet.Remove(this);
//     }
// }
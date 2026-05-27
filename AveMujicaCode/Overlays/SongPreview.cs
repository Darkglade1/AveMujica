using AveMujica.AveMujicaCode.Cards.CardMods;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AveMujica.AveMujicaCode.Overlays;

public partial class SongPreview : Control
{
    public static SongPreview? Instance { get; private set; }

    private NCard? _cardDisplay;

    public override void _Ready()
    {
        Instance = this;
        Name = "SongPreview";
        MouseFilter = MouseFilterEnum.Ignore; 
    }

    public void UpdateDisplay(ICombatState? combatState)
    {
        if (combatState != null)
        {
            if (_cardDisplay != null)
            {
                _cardDisplay.Show();
                var currentSong = ComposeHelper.ComposeFields.CurrentSong.Get(combatState);
                if (currentSong != null)
                {
                    _cardDisplay.Model = currentSong;
                    _cardDisplay.UpdateVisuals(PileType.Deck, CardPreviewMode.Normal);
                }
                else
                {
                    _cardDisplay.Hide();
                }
            }
            else
            {
                SetupDisplay(combatState);
            }
        }
    }

    private void SetupDisplay(ICombatState? combatState)
    {
        if (combatState != null)
        {
            var currentSong = ComposeHelper.ComposeFields.CurrentSong.Get(combatState);
            if (currentSong != null)
            {
                if (NCombatRoom.Instance != null)
                {
                    NCreature? creatureNode = NCombatRoom.Instance.GetCreatureNode(currentSong.Owner.Creature);
                    Marker2D? specialNode = creatureNode?.GetSpecialNode<Marker2D>("%IntentPos");
                    if (specialNode != null)
                    {
                        NCard? nCard = NCard.Create(currentSong);
                        specialNode.AddChildSafely(nCard);
                        if (nCard != null)
                        {
                            nCard.Position += new Vector2(0, -100f); // Settings.scale needed??? idk
                            nCard.Scale = new Vector2(0.5f, 0.5f);
                            nCard.UpdateVisuals(PileType.Deck, CardPreviewMode.Normal);
                            _cardDisplay = nCard;
                            _cardDisplay.Show();
                        }
                    }
                }
            }
        }
    }
}

[HarmonyPatch(typeof(NEnergyCounter), nameof(NEnergyCounter._Ready))]
public static class SongPreviewOverlayPatch
{
    public static void Postfix(NEnergyCounter __instance)
    {
        if (SongPreview.Instance != null) return;
        
        var overlay = new SongPreview();
        __instance.AddChild(overlay);
    }
}
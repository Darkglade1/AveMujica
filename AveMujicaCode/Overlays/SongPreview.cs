using AveMujica.AveMujicaCode.Cards.CardMods;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AveMujica.AveMujicaCode.Overlays;

public partial class SongPreview : Control
{
    public static SongPreview? Instance { get; private set; }

    private Dictionary<Player, NCard?> _cardDisplayDict = new();

    public override void _Ready()
    {
        Instance = this;
        Name = "SongPreview";
        MouseFilter = MouseFilterEnum.Ignore; 
    }

    public void UpdateDisplay(ICombatState? combatState, Player owner)
    {
        if (combatState != null && LocalContext.IsMe(owner))
        {
            if (!_cardDisplayDict.ContainsKey(owner))
            {
                SetupDisplay(combatState, owner);
            }
            var cardDisplay = _cardDisplayDict[owner];
            if (cardDisplay != null)
            {
                cardDisplay.Show();
                var songDict = ComposeHelper.ComposeFields.CurrentSong.Get(combatState);
                if (songDict != null)
                {
                    var currentSong = songDict[owner];
                    if (currentSong != null)
                    {
                        cardDisplay.Model = currentSong;
                        cardDisplay.UpdateVisuals(PileType.Deck, CardPreviewMode.Normal);
                    }
                    else
                    {
                        cardDisplay.Hide();
                    }
                }
            }
            else
            {
                SetupDisplay(combatState, owner);
            }
        }
    }

    private void SetupDisplay(ICombatState? combatState, Player owner)
    {
        if (combatState != null && LocalContext.IsMe(owner))
        {
            var songDict = ComposeHelper.ComposeFields.CurrentSong.Get(combatState);
            if (songDict != null)
            {
                var currentSong = songDict[owner];
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
                                _cardDisplayDict[currentSong.Owner] = nCard;
                                _cardDisplayDict[currentSong.Owner]?.Show();
                            }
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
        var overlay = new SongPreview();
        __instance.AddChild(overlay);
    }
}
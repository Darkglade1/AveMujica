using AveMujica.AveMujicaCode.Cards.CardMods;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AveMujica.AveMujicaCode.Overlays;

public partial class SongPreview : Control
{
    public static SongPreview? Instance { get; private set; }

    private Dictionary<Player, NGridCardHolder?> _cardDisplayDict = new();

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
            if (cardDisplay != null && IsInstanceValid(cardDisplay))
            {
                cardDisplay.Show();
                var songDict = ComposeHelper.ComposeFields.CurrentSong.Get(combatState);
                if (songDict != null)
                {
                    var currentSong = songDict[owner];
                    if (currentSong != null)
                    {
                        cardDisplay.ReassignToCard(currentSong, PileType.None, null, ModelVisibility.Visible);
                        cardDisplay.Show();
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
                            if (nCard != null)
                            {
                                NGridCardHolder? nCardHolder = NGridCardHolder.Create(nCard);
                                if (nCardHolder != null)
                                {
                                    nCardHolder.Position += new Vector2(0, -100f);
                                    nCardHolder.ReassignToCard(currentSong, PileType.None, null, ModelVisibility.Visible);
                                    specialNode.AddChildSafely(nCardHolder);
                                    _cardDisplayDict[currentSong.Owner] = nCardHolder;
                                    _cardDisplayDict[currentSong.Owner]?.Show();
                                }
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
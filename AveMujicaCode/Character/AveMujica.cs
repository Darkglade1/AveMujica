using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace AveMujica.AveMujicaCode.Character;

public class AveMujica : PlaceholderCharacterModel
{
    public const string CharacterId = "AveMujica";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BurningBlood>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<AveMujicaCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<AveMujicaRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<AveMujicaPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        AnimState animState = new AnimState("Idle", isLooping: true);
        // AnimState animState2 = new AnimState("cast");
        // AnimState animState3 = new AnimState("attack");
        // AnimState animState4 = new AnimState("hurt");
        // AnimState state = new AnimState("die");
        // AnimState animState5 = new AnimState("shiv");
        // AnimState animState6 = new AnimState("relaxed_loop", isLooping: true);
        // animState2.NextState = animState;
        // animState3.NextState = animState;
        // animState4.NextState = animState;
        // animState5.NextState = animState;
        // animState6.AddBranch("Idle", animState);
        CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
        creatureAnimator.AddAnyState("Idle", animState);
        // creatureAnimator.AddAnyState("Dead", state);
        // creatureAnimator.AddAnyState("Hit", animState4);
        // creatureAnimator.AddAnyState("Attack", animState3);
        // creatureAnimator.AddAnyState("Cast", animState2);
        // creatureAnimator.AddAnyState("Shiv", animState5);
        // creatureAnimator.AddAnyState("Relaxed", animState6);
        return creatureAnimator;
    }
    
    public override string CustomVisualPath => "res://AveMujica/images/character/oblvns.tscn";

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}
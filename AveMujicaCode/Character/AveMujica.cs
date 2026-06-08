using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Cards.Basic;
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
    public override int StartingHp => 75;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Waltz>(),
        ModelDb.Card<Muse>()
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
        AnimState animState2 = new AnimState("Combat");
        AnimState animState3 = new AnimState("Attack");
        AnimState state = new AnimState("Die");
        animState2.NextState = animState;
        animState3.NextState = animState;
        CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
        creatureAnimator.AddAnyState("Idle", animState);
        creatureAnimator.AddAnyState("Dead", state);
        creatureAnimator.AddAnyState("Attack", animState3);
        creatureAnimator.AddAnyState("Cast", animState2);
        return creatureAnimator;
    }
    
    public override string CustomVisualPath => "oblivionis/oblvns.tscn".CharacterPath();
    public override string CustomMerchantAnimPath => "oblivionis/merchant.tscn".CharacterPath();
    //public override string CustomRestSiteAnimPath => "oblivionis/merchant.tscn".CharacterPath();
    public override string CustomCharacterSelectBg => "selection_screen.tscn".CharacterUiPath();
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}
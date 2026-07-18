using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Cards.Dolls;
using BaseLib.Patches.Features;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AveMujica.AveMujicaCode.Potions;

public class BottledDreams : AveMujicaPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => CustomTargetType.PetOrSelf;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
    ];
    
    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Dreamspin)
    ];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color("3c2d26"));

        await DollHelper.Dreamspin(choiceContext, Owner, target, null);
        await DollHelper.Dreamspin(choiceContext, Owner, target, null);
    }
}
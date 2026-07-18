using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Cards.CardMods;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AveMujica.AveMujicaCode.Potions;

public class JarOfMelodies : AveMujicaPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(3)];
    
    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose)
    ];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        if (target.Player == null) return;
        
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color("3c2d26"));

        for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
        {
            await ComposeHelper.RandomCompose(target.Player, choiceContext, true);
        }
    }
}
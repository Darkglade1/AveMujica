using AveMujica.AveMujicaCode.Enchantments;
using AveMujica.AveMujicaCode.Powers;
using AveMujica.AveMujicaCode.Rewards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class Consign() : AveMujicaCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            var hoverTips = HoverTipFactory.FromEnchantment<Vanishing>();
            hoverTips = hoverTips.AddItem(HoverTipFactory.Static(StaticHoverTip.Fatal));
            return hoverTips;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState?.RunState.CurrentRoom is CombatRoom combatRoom)
        {
            ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
            bool shouldTriggerFatal = play.Target.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());
            AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
            var targetKilled = attackCommand.Results.SelectMany(r => r)
                .Any((Func<DamageResult, bool>)(r => r.WasTargetKilled));
            if (shouldTriggerFatal && targetKilled)
            {
                combatRoom.AddExtraReward(Owner, 
                    new CardEnchantReward(ModelDb.GetId<Vanishing>(), 1, CardEnchantReward.EnchantRewardFilter.CanEnhance,
                        Owner));
                await PowerCmd.Apply<ConsignPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
}
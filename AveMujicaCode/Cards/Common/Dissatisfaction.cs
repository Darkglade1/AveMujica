using AveMujica.AveMujicaCode.Cards.Token;
using AveMujica.AveMujicaCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.Common;

public class Dissatisfaction() : AveMujicaCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Song>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        var card = Owner.Creature.CombatState?.CreateCard<Song>(Owner);
        if (card != null && !CombatManager.Instance.IsOverOrEnding)
        {
            if (Owner.Creature.HasPower<EncorePower>())
            {
                card._baseReplayCount = Owner.Creature.GetPowerAmount<EncorePower>();
            }
            await CardPileCmd.AddGeneratedCardsToCombat([card], PileType.Hand, Owner);
            await Cmd.Wait(0.25f);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}
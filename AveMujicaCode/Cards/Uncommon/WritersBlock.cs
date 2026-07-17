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

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class WritersBlock() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7, ValueProp.Move), new CardsVar(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Song>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
        for (int i = 0; i < DynamicVars.Cards.BaseValue; i++)
        {
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
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}
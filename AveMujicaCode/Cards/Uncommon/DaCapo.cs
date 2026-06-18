using AveMujica.AveMujicaCode.Cards.CardMods;
using AveMujica.AveMujicaCode.Cards.Token;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.Uncommon;

public class DaCapo() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose),
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];
    
    protected override bool ShouldGlowGoldInternal => PlayedSongThisTurn;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await ComposeHelper.RandomCompose(Owner, choiceContext, IsUpgraded);
        if (PlayedSongThisTurn)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            IEnumerable<CardModel> cardToTransfigure = await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1),null,this);
            foreach (CardModel cardModel in cardToTransfigure)
            {
                cardModel.BaseReplayCount++;
                CardCmd.ApplyKeyword(cardModel, CardKeyword.Exhaust);
            }
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
    
    public bool PlayedSongThisTurn
    {
        get
        {
            return CombatManager.Instance.History.CardPlaysFinished.Any(e => e.CardPlay.Card is Song && e.CardPlay.Card.Owner == Owner && e.HappenedThisTurn(CombatState));
        }
    }
}
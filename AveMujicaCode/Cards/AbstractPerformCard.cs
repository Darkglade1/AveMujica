using AveMujica.AveMujicaCode.Hooks;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace AveMujica.AveMujicaCode.Cards;

public abstract class AbstractPerformCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    AveMujicaCard(cost, type, rarity, target)
{
    protected abstract List<CardType[]> PerformSequences();
    protected override bool ShouldGlowGoldInternal => IsPerformActive();

    protected bool IsPerformActive()
    {
        return PerformSequences().Any(IsPerformActiveForSequence);
    }

    protected abstract Task DoPerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes, int numTriggers);
    
    public bool IsPerformActiveForSequence(CardType[] cardTypes)
    {
        var flawlessPerformance = Owner.Creature.GetPower<FlawlessPerformancePower>();
        if (flawlessPerformance != null)
        {
            return true;
        }
        var numSkills = cardTypes.Count(e => e == CardType.Skill);
        var numSkillsPlayedThisTurn = CombatManager.Instance.History.CardPlaysFinished.Count(e =>
            e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Card.Owner == Owner &&
            e.HappenedThisTurn(CombatState));
        
        var numAttacks = cardTypes.Count(e => e == CardType.Attack);
        var numAttacksPlayedThisTurn = CombatManager.Instance.History.CardPlaysFinished.Count(e =>
            e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner == Owner &&
            e.HappenedThisTurn(CombatState));

        return numSkillsPlayedThisTurn >= numSkills && numAttacksPlayedThisTurn >= numAttacks;
    }

    protected async Task ExecutePerformEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes)
    {
        if (IsPerformActiveForSequence(cardTypes))
        {
            var numTriggers = 1;
            await DoPerformEffect(choiceContext, play, cardTypes, numTriggers);
            for (int i = 0; i < numTriggers; i++)
            {
                await AveMujicaHooks.AfterPerform(Owner.RunState, Owner.Creature.CombatState, choiceContext, play);
                ICombatState? combatState = CombatState ?? Owner.Creature.CombatState;
                if (!CombatManager.Instance.IsOverOrEnding && combatState != null)
                {
                    CombatManager.Instance.History.Add(combatState, new PerformCardEntry(this, combatState.RoundNumber, combatState.CurrentSide, CombatManager.Instance.History, combatState.Players));
                }
            }
        }
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Perform)
    ];
}
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using AveMujica.AveMujicaCode.Character;
using AveMujica.AveMujicaCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards;

public abstract class RhythmCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    AveMujicaCard(cost, type, rarity, target)
{
    protected abstract List<CardType[]> RhythmSequences();
    protected override bool ShouldGlowGoldInternal => IsRhythmActive();

    protected bool IsRhythmActive()
    {
        return RhythmSequences().Any(IsRhythmActiveForSequence);
    }

    protected abstract Task DoRhythmEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes);
    
    public static bool IsRhythmActiveForSequence(CardType[] cardTypes)
    {
        List<CardPlayFinishedEntry> cardHistoryList = CombatManager.Instance.History.CardPlaysFinished.ToList();
        if (cardHistoryList.Count < cardTypes.Length)
        {
            return false;
        }
        int cardHistoryListIndex = cardHistoryList.Count - 1;
        for (int i = cardTypes.Length - 1; i >= 0; i--)
        {
            CardModel card = cardHistoryList[cardHistoryListIndex].CardPlay.Card;
            if (card.Type != cardTypes[i])
            {
                return false;
            }
            cardHistoryListIndex--;
        }
        return true;
    }

    protected async Task ExecuteRhythmEffect(PlayerChoiceContext choiceContext, CardPlay play, CardType[] cardTypes)
    {
        if (IsRhythmActiveForSequence(cardTypes))
        {
            await DoRhythmEffect(choiceContext, play, cardTypes);
        }
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Rhythm)
    ];
}
using AveMujica.AveMujicaCode.Cards.CardMods;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace AveMujica.AveMujicaCode.Cards.Token;

public class Song() : AveMujicaCard(0,
    CardType.Skill, CardRarity.Token,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
    }

    protected override void OnUpgrade()
    {

    }

    public async Task OnSongSelected(IReadOnlyCollection<CardModifier> mods)
    {
        CardModel? currentSong = ComposeHelper.ComposeFields.CurrentSong.Get(Owner);
        if (currentSong == null)
        {
            Song newSong = Owner.Creature.CombatState.CreateCard<Song>(Owner);
            currentSong = newSong;
        }

        foreach (var cardMod in mods)
        {
            CardModifier.AddModifier(currentSong, cardMod);
        }

        var numComposes = ComposeHelper.ComposeFields.CurrentComposeNum.Get(Owner);
        numComposes++;
        if (numComposes >= ComposeHelper.NumComposesSongComplete)
        {
            await CardPileCmd.AddGeneratedCardToCombat(currentSong, PileType.Hand, Owner);
            ComposeHelper.ComposeFields.CurrentSong.Set(Owner, null);
            ComposeHelper.ComposeFields.CurrentComposeNum.Set(Owner, 0);
        }
        else
        {
            ComposeHelper.ComposeFields.CurrentSong.Set(Owner, currentSong);
            ComposeHelper.ComposeFields.CurrentComposeNum.Set(Owner, numComposes);
        }
    }
}
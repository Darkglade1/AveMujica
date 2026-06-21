using AveMujica.AveMujicaCode.Audio;
using AveMujica.AveMujicaCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AveMujica.AveMujicaCode.Cards.Ancient;

public class GoddessOfOblivion() : AveMujicaCard(2,
    CardType.Power, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Oblivion>(10), new PowerVar<GoddessOfOblivionPower>(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Sfx.SKILL_KEYBOARD2.Play();
        await PowerCmd.Apply<Oblivion>(choiceContext, Owner.Creature, DynamicVars["Oblivion"].BaseValue, Owner.Creature,this);
        await PowerCmd.Apply<GoddessOfOblivionPower>(choiceContext, Owner.Creature, DynamicVars["GoddessOfOblivionPower"].BaseValue, Owner.Creature,this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Oblivion"].UpgradeValueBy(3);
        DynamicVars["GoddessOfOblivionPower"].UpgradeValueBy(1);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<Oblivion>(),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];
}
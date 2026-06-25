using AveMujica.AveMujicaCode.Cards.CardMods;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class Sophie() : AveMujicaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(35), new PowerVar<VulnerablePower>(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Compose),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, Owner.Creature,  DynamicVars["VulnerablePower"].IntValue, Owner.Creature, this);
        var enemyLoseHPMod = (EnemyLoseHPMod)ModelDb.Get<EnemyLoseHPMod>().MutableClone();
        enemyLoseHPMod.HPLossAmt = (int)DynamicVars["HpLoss"].BaseValue;
        await ComposeHelper.AddComposeEffectsToSong([enemyLoseHPMod], Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.HpLoss.UpgradeValueBy(15);
    }
}
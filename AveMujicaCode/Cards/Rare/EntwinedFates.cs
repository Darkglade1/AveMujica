using AveMujica.AveMujicaCode.Cards.Dolls;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AveMujica.AveMujicaCode.Cards.Rare;

public class EntwinedFates() : AveMujicaCard(3,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower(ModelDb.Power<StrengthPower>()),
        HoverTipFactory.FromPower(ModelDb.Power<DexterityPower>()),
        HoverTipFactory.FromKeyword(AveMujicaKeywords.Doll)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState != null)
        {
            foreach (var doll in CombatState.Allies)
            {
                if (doll.Monster is AbstractDoll && doll.PetOwner == Owner && doll.IsAlive)
                {
                    await PowerCmd.Apply<StrengthPower>(choiceContext, doll, DynamicVars["StrengthPower"].BaseValue, Owner.Creature,this);
                    await PowerCmd.Apply<DexterityPower>(choiceContext, doll, DynamicVars["StrengthPower"].BaseValue, Owner.Creature,this);   
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
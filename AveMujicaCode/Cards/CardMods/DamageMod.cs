using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class DamageMod : CardModifier
{
    public int DamageAmt { get; set; }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner)
        {
            MainFile.Logger.Info("Hook called for " + Id);
            ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
            await DamageCmd.Attack(DamageAmt).FromCard(Owner).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"Deal {DamageAmt} damage. \n";
    }
}
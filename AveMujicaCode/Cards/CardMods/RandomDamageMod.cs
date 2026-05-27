using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class RandomDamageMod : CardModifier
{
    public int DamageAmt { get; set; }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && Owner.CombatState != null && play.Card == Owner)
        {
            MainFile.Logger.Info("Hook called for " + Id);
            await DamageCmd.Attack(DamageAmt).FromCard(Owner).TargetingRandomOpponents(Owner.CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"Deal {DamageAmt} damage to a random enemy. \n";
    }
}
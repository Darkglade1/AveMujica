using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class RandomDamageMod : CardModifier
{
    public RandomDamageMod()
    {
        Priority = -25;
    }
    public int DamageAmt { get; set; }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && Owner.CombatState != null && play.Card == Owner)
        {
            await DamageCmd.Attack(DamageAmt).FromCard(Owner).TargetingRandomOpponents(Owner.CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        }
    }
    
    public override void OnInitialApplication()
    {
        if (Owner is Song song)
        {
            song.IsAttack = true;
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"Deal {DamageAmt} damage to a random enemy." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
    }
}
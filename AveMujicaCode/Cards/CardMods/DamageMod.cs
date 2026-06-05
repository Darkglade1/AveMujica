using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class DamageMod : CardModifier
{
    public DamageMod()
    {
        Priority = -50;
    }
    public DynamicVar? DamageVar
    {
        get;
        set;
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Owner != null && play.Card == Owner && DamageVar != null)
        {
            ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
            await DamageCmd.Attack(DamageVar.BaseValue).FromCard(Owner).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        }
    }
    
    public override void OnInitialApplication()
    {
        if (Owner is Song song)
        {
            song.IsAttack = true;
            song.IsTargeted = true;
        }
    }
    
    public override void ModifyDescription(Creature? target, ref string description)
    {
        if (DamageVar != null)
        {
            var roundedDamage = Math.Floor(DamageVar.PreviewValue);
            if (roundedDamage != DamageVar.BaseValue)
            {
                if (roundedDamage > DamageVar.BaseValue)
                {
                    description += $"Deal [green]{roundedDamage}[/green] damage." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
                }
                else
                {
                    description += $"Deal [red]{roundedDamage}[/red] damage." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
                }
            }
            else
            {
                description += $"Deal {DamageVar.BaseValue} damage." + ComposeHelper.GetNewLineIfNotLastCardMod(this);
            }
        }
    }
}
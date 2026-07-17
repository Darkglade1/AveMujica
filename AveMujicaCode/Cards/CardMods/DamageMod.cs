using AveMujica.AveMujicaCode.Cards.Token;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace AveMujica.AveMujicaCode.Cards.CardMods;

public class DamageMod : CardModifier
{
    private string locString;
    public DamageMod()
    {
        Priority = -50;
        locString = new LocString("card_mods", "AVEMUJICA-DAMAGE-MOD.description").GetRawText();
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
            await DamageCmd.Attack(DamageVar.BaseValue).FromCard(Owner, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        }
    }
    
    public override void OnInitialApplication()
    {
        DamageVar = new DamageVar(Amount, ValueProp.Move);
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
                    var greenDamage = $"[green]{roundedDamage}[/green]";
                    description += String.Format(locString, greenDamage) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
                }
                else
                {
                    var redDamage = $"[red]{roundedDamage}[/red]";
                    description += String.Format(locString, redDamage) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
                }
            }
            else
            {
                description += String.Format(locString, DamageVar.BaseValue) + ComposeHelper.GetNewLineIfNotLastCardMod(this);
            }
        }
    }
}
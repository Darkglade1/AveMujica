using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Cards.Allies;
using AveMujica.AveMujicaCode.Cards.Uncommon;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace AveMujica.AveMujicaCode.Powers;
public class DreamThreadPower : AveMujicaPower
{
    public static int AwakenIncrement = 3;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AveMujicaKeywords.Awaken)];
    
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (Owner.CombatState != null && power is DreamThreadPower && Owner == applier && Owner.Player != null)
        {
            var numTriggers = Amount / AwakenIncrement;
            if (numTriggers > 0)
            {
                for (int i = 0; i < numTriggers; i++)
                {
                    var doloris = (CardModel)ModelDb.Card<Doloris>().MutableClone();
                    var mortis = (CardModel)ModelDb.Card<Mortis>().MutableClone();
                    var timoris = (CardModel)ModelDb.Card<Timoris>().MutableClone();
                    var amoris = (CardModel)ModelDb.Card<Amoris>().MutableClone();
                    List<CardModel> dolls =
                    [
                        doloris,
                        mortis,
                        timoris,
                        amoris
                    ];
                    foreach (CardModel card in dolls)
                    {
                        if (Owner.CombatState != null)
                        {
                            Owner.CombatState.AddCard(card, Owner.Player);
                        }
                    }
                    foreach (var creature in Owner.CombatState.Allies)
                    {
                        if (creature.IsPet && creature.PetOwner == Owner.Player && creature.IsAlive)
                        {
                            if (creature.Monster is DolorisAlly)
                            {
                                dolls.Remove(doloris);
                            }

                            if (creature.Monster is MortisAlly)
                            {
                                dolls.Remove(mortis);
                            }

                            if (creature.Monster is TimorisAlly)
                            {
                                dolls.Remove(timoris);
                            }

                            if (creature.Monster is AmorisAlly)
                            {
                                dolls.Remove(amoris);
                            }
                        }
                    }

                    if (dolls.Count > 0)
                    {
                        dolls.StableShuffle(Owner.Player.RunState.Rng.CombatCardGeneration);
                        while (dolls.Count > 2)
                        {
                            dolls.RemoveAt(dolls.Count - 1);
                        }

                        var pickedCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, dolls, Owner.Player);
                        if (pickedCard != null)
                        {
                            if (pickedCard is Doloris)
                            {
                                await AllyHelper.Awaken<DolorisAlly>(choiceContext, Owner.Player, 1);
                            }
                            if (pickedCard is Mortis)
                            {
                                await AllyHelper.Awaken<MortisAlly>(choiceContext, Owner.Player, 1);
                            }
                            if (pickedCard is Timoris)
                            {
                                await AllyHelper.Awaken<TimorisAlly>(choiceContext, Owner.Player, 1);
                            }
                            if (pickedCard is Amoris)
                            {
                                await AllyHelper.Awaken<AmorisAlly>(choiceContext, Owner.Player, 1);
                            }
                            Amount %= AwakenIncrement;
                            InvokeDisplayAmountChanged();
                        }
                    }
                }
            }
        }
    }
}
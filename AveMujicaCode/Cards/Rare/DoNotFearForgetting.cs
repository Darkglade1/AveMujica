// using AveMujica.AveMujicaCode.Powers;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.Models;
//
// namespace AveMujica.AveMujicaCode.Cards.Rare;
//
// public class DoNotFearForgetting() : AveMujicaCard(2,
//     CardType.Skill, CardRarity.Rare,
//     TargetType.Self)
// {
//     protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Oblivion>(2)];
//     
//     protected override HashSet<CardTag> CanonicalTags => [AveMujicaCardTags.GainsOblivion];
//     
//     public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
//     
//     protected override IEnumerable<IHoverTip> ExtraHoverTips => [
//         HoverTipFactory.FromPower(ModelDb.Power<Oblivion>()),
//         HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
//     ];
//
//     protected override async Task OnPlay(
//         PlayerChoiceContext choiceContext,
//         CardPlay play)
//     {
//         List<CardModel> list = PileType.Hand.GetPile(Owner).Cards.ToList();
//         int cardCount = list.Count;
//         foreach (CardModel card in list)
//         {
//             await CardCmd.Exhaust(choiceContext, card);
//         }
//         for (int i = 0; i < cardCount; i++)
//         {
//             await PowerCmd.Apply<Oblivion>(choiceContext, Owner.Creature, DynamicVars["Oblivion"].BaseValue, Owner.Creature, this);
//         }
//         
//     }
//
//     protected override void OnUpgrade()
//     {
//         DynamicVars["Oblivion"].UpgradeValueBy(2);
//     }
// }
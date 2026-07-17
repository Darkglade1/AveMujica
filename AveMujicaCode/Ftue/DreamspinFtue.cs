using AveMujica.AveMujicaCode.Cards;
using AveMujica.AveMujicaCode.Extensions;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace AveMujica.AveMujicaCode.Ftue;

public class DreamspinFtue() : CustomSingletonModel(HookType.Combat)
{
	public override Task AfterCardDrawn(
		PlayerChoiceContext choiceContext,
		CardModel card,
		bool fromHandDraw)
	{
		if (Config.ViewDreamspinFtue && LocalContext.IsMe(card.Owner) &&
		    card.Tags.Contains(AveMujicaCardTags.PerformsDreamspin))
		{
			return TaskHelper.RunSafely(ShowCombatFtue());
		}
		return Task.CompletedTask;
	}

	private static async Task ShowCombatFtue()
	{
		SceneTree? tree = NModalContainer.Instance != null ? NModalContainer.Instance.GetTree() : null;
		if (tree == null)
		{
			return;
		}
		if (((NModalContainer.Instance != null) ? NModalContainer.Instance.OpenModal : null) == null)
		{
			NDreamspinFtue nDreamspinFtue = NDreamspinFtue.Create("Dreamspin", ["AVEMUJICA-DREAMSPIN-FTUE.title", "AVEMUJICA-DREAMSPIN-FTUE.title"], ["AVEMUJICA-DREAMSPIN-FTUE.body", "AVEMUJICA-DREAMSPIN-FTUE.body"], ["ftue/dreamspin_ftue.png".ImagePath(), "ftue/dreamspin_ftue2.png".ImagePath()]);
			if (NModalContainer.Instance != null)
			{
				NModalContainer.Instance.Add(nDreamspinFtue);
				Config.ViewDreamspinFtue = false;
			}
		}
	}
}
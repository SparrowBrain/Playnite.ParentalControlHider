using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public class ManagedGamesFilter
	{
		public bool IsGameManagedByParentalHider(Game game, Tag parentalHiderTag)
		{
			return !game.Hidden || game.Hidden && game.TagIds.Contains(parentalHiderTag.Id);
		}
	}
}
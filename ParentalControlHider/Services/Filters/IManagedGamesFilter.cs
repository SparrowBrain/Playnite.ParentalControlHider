using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public interface IManagedGamesFilter
	{
		bool IsGameManagedByParentalHider(Game game, Tag parentalHiderTag);
	}
}
using System.Linq;
using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public class GenresBlacklist : IGenresBlacklist
	{
		public bool DoesItContainBlacklistedGenre(Game game, ParentalControlHiderSettings settings)
		{
			return game.GenreIds?.Any(x => settings.BlacklistedGenreIds.Contains(x)) ?? false;
		}
	}
}
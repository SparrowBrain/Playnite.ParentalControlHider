using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public interface IGenresBlacklist
	{
		bool DoesItContainBlacklistedGenre(Game game, ParentalControlHiderSettings settings);
	}
}
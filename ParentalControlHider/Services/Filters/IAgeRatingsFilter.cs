using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public interface IAgeRatingsFilter
	{
		bool IsAgeAllowed(Game game, ParentalControlHiderSettings settings);
	}
}
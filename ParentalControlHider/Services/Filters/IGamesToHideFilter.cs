using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public interface IGamesToHideFilter
	{
		bool ShouldHideTheGame(Game game, ParentalControlHiderSettings settings);
	}
}
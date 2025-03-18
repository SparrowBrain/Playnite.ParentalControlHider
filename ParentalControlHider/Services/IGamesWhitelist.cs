using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services
{
	public interface IGamesWhitelist
	{
		bool IsOnWhitelist(Game game, ParentalControlHiderSettings settings);
	}
}
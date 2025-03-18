using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services
{
	public class GamesWhitelist : IGamesWhitelist
	{
		public bool IsOnWhitelist(Game game, ParentalControlHiderSettings settings)
		{
			return settings.WhitelistedGameIds?.Contains(game.Id) ?? false;
		}
	}
}
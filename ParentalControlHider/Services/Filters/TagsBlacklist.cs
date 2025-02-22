using System.Linq;
using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public class TagsBlacklist
	{
		public bool DoesItContainBlacklistedTag(Game game, ParentalControlHiderSettings settings)
		{
			return game.TagIds.Any(x => settings.BlacklistedTagIds.Contains(x));
		}
	}
}
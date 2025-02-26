using System.Linq;
using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public class TagsBlacklist : ITagsBlacklist
	{
		public bool DoesItContainBlacklistedTag(Game game, ParentalControlHiderSettings settings)
		{
			return game.TagIds.Any(x => settings.BlacklistedTagIds.Contains(x));
		}
	}
}
using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public interface ITagsBlacklist
	{
		bool DoesItContainBlacklistedTag(Game game, ParentalControlHiderSettings settings);
	}
}
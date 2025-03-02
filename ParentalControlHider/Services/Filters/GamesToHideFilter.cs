using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public class GamesToHideFilter : IGamesToHideFilter
	{
		private readonly IAgeRatingsFilter _ageRatingsFilter;
		private readonly ITagsBlacklist _tagsBlacklist;

		public GamesToHideFilter(IAgeRatingsFilter ageRatingsFilter, ITagsBlacklist tagsBlacklist)
		{
			_ageRatingsFilter = ageRatingsFilter;
			_tagsBlacklist = tagsBlacklist;
		}

		public bool ShouldHideTheGame(Game game, ParentalControlHiderSettings settings)
		{
			return !_ageRatingsFilter.IsAgeAllowed(game, settings)
			       || _tagsBlacklist.DoesItContainBlacklistedTag(game, settings);
		}
	}
}
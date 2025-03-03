using ParentalControlHider.Settings;
using Playnite.SDK.Models;

namespace ParentalControlHider.Services.Filters
{
	public class GamesToHideFilter : IGamesToHideFilter
	{
		private readonly IAgeRatingsFilter _ageRatingsFilter;
		private readonly ITagsBlacklist _tagsBlacklist;
		private readonly IGenresBlacklist _genresBlacklist;

		public GamesToHideFilter(
			IAgeRatingsFilter ageRatingsFilter,
			ITagsBlacklist tagsBlacklist,
			IGenresBlacklist genresBlacklist)
		{
			_ageRatingsFilter = ageRatingsFilter;
			_tagsBlacklist = tagsBlacklist;
			_genresBlacklist = genresBlacklist;
		}

		public bool ShouldHideTheGame(Game game, ParentalControlHiderSettings settings)
		{
			return !_ageRatingsFilter.IsAgeAllowed(game, settings)
				   || _tagsBlacklist.DoesItContainBlacklistedTag(game, settings)
				   || _genresBlacklist.DoesItContainBlacklistedGenre(game, settings);
		}
	}
}
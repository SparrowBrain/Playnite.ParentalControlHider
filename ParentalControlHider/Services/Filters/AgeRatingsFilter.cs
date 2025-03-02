using ParentalControlHider.Infrastructure;
using ParentalControlHider.Settings;
using Playnite.SDK.Models;
using System.Collections.Generic;
using System.Linq;

namespace ParentalControlHider.Services.Filters
{
	public class AgeRatingsFilter : IAgeRatingsFilter
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public AgeRatingsFilter(IDateTimeProvider dateTimeProvider)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public bool IsAgeAllowed(Game game, ParentalControlHiderSettings settings)
		{
			var allowedAges = game.AgeRatingIds
				?.Where(x => settings.UsedAgeRatings.Contains(x))
				.Select(x => settings.AgeRatingsWithAge[x])
				.ToList() ?? new List<int>();

			if (!allowedAges.Any())
			{
				// Permissive
				return true;
			}

			// Permissive
			var allowedAge = allowedAges.Min();

			var allowedFrom = settings.Birthday.AddYears(allowedAge);
			return _dateTimeProvider.Now >= allowedFrom;
		}
	}
}
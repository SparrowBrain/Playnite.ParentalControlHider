using System;
using System.Collections.Generic;

namespace ParentalControlHider.Settings
{
	public class ParentalControlHiderSettings : ObservableObject
	{
		public static ParentalControlHiderSettings Default => new ParentalControlHiderSettings
		{
			Birthday = DateTime.Now
		};

		public DateTime Birthday { get; set; }

		public Dictionary<Guid, int> AgeRatingsWithAge { get; set; } = new Dictionary<Guid, int>();

		public HashSet<Guid> UsedAgeRatings { get; set; } = new HashSet<Guid>();

		public HashSet<Guid> BlacklistedTagIds { get; set; } = new HashSet<Guid>();

		public HashSet<Guid> GameWhitelist { get; set; } = new HashSet<Guid>();
	}
}
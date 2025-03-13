using System;
using System.Collections.Generic;

namespace ParentalControlHider.Settings
{
	public class ParentalControlHiderSettings : ObservableObject
	{
		public static ParentalControlHiderSettings Default => new ParentalControlHiderSettings
		{
			Birthday = DateTime.Now,
			RunOnApplicationStarted = true,
			RunOnLibraryUpdated = true,
			RunAfterUnhidden = true,
			MinutesToRunAfterUnhidden = 30,
		};

		public bool RunOnApplicationStarted { get; set; }

		public bool RunOnLibraryUpdated { get; set; }

		public bool RunAfterUnhidden { get; set; }

		public int MinutesToRunAfterUnhidden { get; set; }

		public DateTime Birthday { get; set; }

		public Dictionary<Guid, int> AgeRatingsWithAge { get; set; } = new Dictionary<Guid, int>();

		public HashSet<Guid> UsedAgeRatings { get; set; } = new HashSet<Guid>();

		public HashSet<Guid> BlacklistedTagIds { get; set; } = new HashSet<Guid>();

		public HashSet<Guid> BlacklistedGenreIds { get; set; } = new HashSet<Guid>();

		public HashSet<Guid> WhitelistedGameIds { get; set; } = new HashSet<Guid>();
	}
}
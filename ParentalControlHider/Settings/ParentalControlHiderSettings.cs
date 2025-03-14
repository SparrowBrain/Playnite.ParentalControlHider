using System;
using System.Collections.Generic;

namespace ParentalControlHider.Settings
{
	public class ParentalControlHiderSettings : ObservableObject
	{
		private bool _runOnApplicationStarted;
		private bool _runOnLibraryUpdated;
		private bool _runAfterUnhidden;
		private int _minutesToRunAfterUnhidden;

		public static ParentalControlHiderSettings Default => new ParentalControlHiderSettings
		{
			Birthday = DateTime.Now,
			RunOnApplicationStarted = true,
			RunOnLibraryUpdated = true,
			RunAfterUnhidden = true,
			MinutesToRunAfterUnhidden = 30,
		};

		public bool RunOnApplicationStarted
		{
			get => _runOnApplicationStarted;
			set => SetValue(ref _runOnApplicationStarted, value);
		}

		public bool RunOnLibraryUpdated
		{
			get => _runOnLibraryUpdated;
			set => SetValue(ref _runOnLibraryUpdated, value);
		}

		public bool RunAfterUnhidden
		{
			get => _runAfterUnhidden;
			set => SetValue(ref _runAfterUnhidden, value);
		}

		public int MinutesToRunAfterUnhidden
		{
			get => _minutesToRunAfterUnhidden;
			set => SetValue(ref _minutesToRunAfterUnhidden, value);
		}

		public DateTime Birthday { get; set; }

		public Dictionary<Guid, int> AgeRatingsWithAge { get; set; } = new Dictionary<Guid, int>();

		public HashSet<Guid> UsedAgeRatings { get; set; } = new HashSet<Guid>();

		public HashSet<Guid> BlacklistedTagIds { get; set; } = new HashSet<Guid>();

		public HashSet<Guid> BlacklistedGenreIds { get; set; } = new HashSet<Guid>();

		public HashSet<Guid> WhitelistedGameIds { get; set; } = new HashSet<Guid>();
	}
}
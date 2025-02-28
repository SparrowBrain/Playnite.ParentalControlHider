using Playnite.SDK;
using Playnite.SDK.Data;
using Playnite.SDK.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ParentalControlHider.Settings.MVVM
{
	public class ParentalControlHiderSettingsViewModel : ObservableObject, ISettings
	{
		private readonly ILogger _logger = LogManager.GetLogger();
		private readonly ParentalControlHider _plugin;

		private ParentalControlHiderSettings _editingClone;
		private ParentalControlHiderSettings _settings;
		private ObservableCollection<AgeRatingsViewModel> _ageRatings = new ObservableCollection<AgeRatingsViewModel>();
		private ObservableCollection<Tag> _allowedTags = new ObservableCollection<Tag>();
		private ObservableCollection<Tag> _blacklistedTags = new ObservableCollection<Tag>();
		private Tag _selectedAllowedTag;
		private Tag _selectedBlacklistedTag;
		private string _allowedTagsFilter;
		private string _blacklistedTagsFilter;

		public ParentalControlHiderSettingsViewModel(ParentalControlHider plugin)
		{
			_plugin = plugin;
			var savedSettings = plugin.LoadPluginSettings<ParentalControlHiderSettings>();

			Settings = savedSettings ?? new ParentalControlHiderSettings();
			InitializeAgeRatings();
			InitializeTags();
		}

		public ParentalControlHiderSettings Settings
		{
			get => _settings;
			set
			{
				_settings = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<AgeRatingsViewModel> AgeRatings
		{
			get => _ageRatings;
			set => SetValue(ref _ageRatings, value);
		}

		public ObservableCollection<Tag> AllowedTags
		{
			get => _allowedTags;
			set => SetValue(ref _allowedTags, value);
		}

		public Tag SelectedAllowedTag
		{
			get => _selectedAllowedTag;
			set => SetValue(ref _selectedAllowedTag, value);
		}

		public string AllowedTagsFilter
		{
			get => _allowedTagsFilter;
			set
			{
				SetValue(ref _allowedTagsFilter, value);
				InitializeTags();
			}
		}

		public ObservableCollection<Tag> BlacklistedTags
		{
			get => _blacklistedTags;
			set => SetValue(ref _blacklistedTags, value);
		}

		public Tag SelectedBlacklistedTag
		{
			get => _selectedBlacklistedTag;
			set => SetValue(ref _selectedBlacklistedTag, value);
		}

		public string BlacklistedTagsFilter
		{
			get => _blacklistedTagsFilter;
			set
			{
				SetValue(ref _blacklistedTagsFilter, value);
				InitializeTags();
			}
		}

		public ICommand BlacklistTagCommand => new RelayCommand(() =>
		{
			if (SelectedAllowedTag != null)
			{
				Settings.BlacklistedTagIds.Add(SelectedAllowedTag.Id);
				InitializeTags();
				SelectedAllowedTag = null;
			}
		});

		public ICommand AllowTagCommand => new RelayCommand(() =>
		{
			if (SelectedBlacklistedTag != null)
			{
				Settings.BlacklistedTagIds.Remove(SelectedBlacklistedTag.Id);
				InitializeTags();
				SelectedBlacklistedTag = null;
			}
		});

		public void BeginEdit()
		{
			_editingClone = Serialization.GetClone(Settings);
		}

		public void CancelEdit()
		{
			Settings = _editingClone;
			InitializeAgeRatings();
			InitializeTags();
		}

		public void EndEdit()
		{
			_plugin.SavePluginSettings(Settings);
		}

		public bool VerifySettings(out List<string> errors)
		{
			// Code execute when user decides to confirm changes made since BeginEdit was called.
			// Executed before EndEdit is called and EndEdit is not called if false is returned.
			// List of errors is presented to user if verification fails.
			errors = new List<string>();
			return true;
		}

		private void InitializeAgeRatings()
		{
			var ratings = new List<AgeRatingsViewModel>();
			var allRatings = _plugin.PlayniteApi.Database.AgeRatings.ToList();

			foreach (var rating in allRatings)
			{
				if (!Settings.AgeRatingsWithAge.TryGetValue(rating.Id, out var age))
				{
					age = 0;
				}

				ratings.Add(new AgeRatingsViewModel(rating.Id, Settings.UsedAgeRatings.Contains(rating.Id), rating.Name, age, Settings));
			}

			AgeRatings = ratings.OrderBy(x => x.Name).ToObservable();
		}

		private void InitializeTags()
		{
			var allTags = _plugin.PlayniteApi.Database.Tags.ToList();

			BlacklistedTags = allTags
				.Where(x => Settings.BlacklistedTagIds.Contains(x.Id)
					&& (string.IsNullOrEmpty(BlacklistedTagsFilter) || x.Name.ToLower().Contains(BlacklistedTagsFilter.ToLower())))
				.OrderBy(x => x.Name)
				.ToObservable();

			AllowedTags = allTags.Where(x => !Settings.BlacklistedTagIds.Contains(x.Id)
					&& (string.IsNullOrEmpty(AllowedTagsFilter) || x.Name.ToLower().Contains(AllowedTagsFilter.ToLower())))
				.OrderBy(x => x.Name)
				.ToObservable();
		}
	}
}
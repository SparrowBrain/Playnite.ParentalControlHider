using ParentalControlHider.Services;
using Playnite.SDK;
using Playnite.SDK.Data;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ParentalControlHider.Settings.MVVM
{
	public class ParentalControlHiderSettingsViewModel : ObservableObject, ISettings
	{
		private readonly ParentalControlHider _plugin;
		private readonly IAgeRatingsAgeProvider _ageRatingsAgeProvider;

		private ParentalControlHiderSettings _editingClone;
		private ParentalControlHiderSettings _settings;
		private string _birthday;
		private ObservableCollection<AgeRatingsViewModel> _ageRatings = new ObservableCollection<AgeRatingsViewModel>();
		private ObservableCollection<Tag> _allowedTags = new ObservableCollection<Tag>();
		private ObservableCollection<Tag> _blacklistedTags = new ObservableCollection<Tag>();
		private Tag _selectedAllowedTag;
		private Tag _selectedBlacklistedTag;
		private string _allowedTagsFilter;
		private string _blacklistedTagsFilter;
		private ObservableCollection<Genre> _allowedGenres = new ObservableCollection<Genre>();
		private ObservableCollection<Genre> _blacklistedGenres = new ObservableCollection<Genre>();
		private Genre _selectedAllowedGenre;
		private Genre _selectedBlacklistedGenre;
		private string _allowedGenresFilter;
		private string _blacklistedGenresFilter;

		public ParentalControlHiderSettingsViewModel(ParentalControlHider plugin, IAgeRatingsAgeProvider ageRatingsAgeProvider)
		{
			_plugin = plugin;
			_ageRatingsAgeProvider = ageRatingsAgeProvider;
			var savedSettings = plugin.LoadPluginSettings<ParentalControlHiderSettings>();

			Settings = savedSettings ?? new ParentalControlHiderSettings();
			InitializeBirthday();
			InitializeAgeRatings();
			InitializeTags();
			InitializeGenres();
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

		public string Birthday
		{
			get => _birthday;
			set => SetValue(ref _birthday, value);
		}

		public ObservableCollection<AgeRatingsViewModel> AgeRatings
		{
			get => _ageRatings;
			set
			{
				SetValue(ref _ageRatings, value);
				OnPropertyChanged();
			}
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

		public ICommand ClearAllowedTagsFilter => new RelayCommand(() => AllowedTagsFilter = string.Empty);

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

		public ICommand ClearBlacklistedTagsFilter => new RelayCommand(() => BlacklistedTagsFilter = string.Empty);

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

		public ObservableCollection<Genre> AllowedGenres
		{
			get => _allowedGenres;
			set => SetValue(ref _allowedGenres, value);
		}

		public Genre SelectedAllowedGenre
		{
			get => _selectedAllowedGenre;
			set => SetValue(ref _selectedAllowedGenre, value);
		}

		public string AllowedGenresFilter
		{
			get => _allowedGenresFilter;
			set
			{
				SetValue(ref _allowedGenresFilter, value);
				InitializeGenres();
			}
		}

		public ICommand ClearAllowedGenresFilter => new RelayCommand(() => AllowedGenresFilter = string.Empty);

		public ObservableCollection<Genre> BlacklistedGenres
		{
			get => _blacklistedGenres;
			set => SetValue(ref _blacklistedGenres, value);
		}

		public Genre SelectedBlacklistedGenre
		{
			get => _selectedBlacklistedGenre;
			set => SetValue(ref _selectedBlacklistedGenre, value);
		}

		public string BlacklistedGenresFilter
		{
			get => _blacklistedGenresFilter;
			set
			{
				SetValue(ref _blacklistedGenresFilter, value);
				InitializeGenres();
			}
		}

		public ICommand ClearBlacklistedGenresFilter => new RelayCommand(() => BlacklistedGenresFilter = string.Empty);

		public ICommand BlacklistGenreCommand => new RelayCommand(() =>
		{
			if (SelectedAllowedGenre != null)
			{
				Settings.BlacklistedGenreIds.Add(SelectedAllowedGenre.Id);
				InitializeGenres();
				SelectedAllowedGenre = null;
			}
		});

		public ICommand AllowGenreCommand => new RelayCommand(() =>
		{
			if (SelectedBlacklistedGenre != null)
			{
				Settings.BlacklistedGenreIds.Remove(SelectedBlacklistedGenre.Id);
				InitializeGenres();
				SelectedBlacklistedGenre = null;
			}
		});

		public void BeginEdit()
		{
			_editingClone = Serialization.GetClone(Settings);
		}

		public void CancelEdit()
		{
			Settings = _editingClone;
			InitializeBirthday();
			InitializeAgeRatings();
			InitializeTags();
			InitializeGenres();
		}

		public void EndEdit()
		{
			Settings.Birthday = DateTime.Parse(Birthday);
			_plugin.SavePluginSettings(Settings);
		}

		public bool VerifySettings(out List<string> errors)
		{
			errors = new List<string>();
			if (!DateTime.TryParse(Birthday, out _))
			{
				errors.Add(ResourceProvider.GetString("LOC_ParentalControlHider_Settings_Error_Birthday"));
			}

			return errors.Count == 0;
		}

		private void InitializeBirthday()
		{
			Birthday = Settings.Birthday.ToShortDateString();
		}

		private void InitializeAgeRatings()
		{
			var ratings = new List<AgeRatingsViewModel>();
			var allRatings = _plugin.PlayniteApi.Database.AgeRatings?.ToList() ?? new List<AgeRating>();

			foreach (var rating in allRatings)
			{
				if (!Settings.AgeRatingsWithAge.TryGetValue(rating.Id, out var age) || age == 0)
				{
					age = _ageRatingsAgeProvider.GetAge(rating.Name);
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

		private void InitializeGenres()
		{
			var allGenres = _plugin.PlayniteApi.Database.Genres.ToList();

			BlacklistedGenres = allGenres
				.Where(x => Settings.BlacklistedGenreIds.Contains(x.Id)
							&& (string.IsNullOrEmpty(BlacklistedGenresFilter) || x.Name.ToLower().Contains(BlacklistedGenresFilter.ToLower())))
				.OrderBy(x => x.Name)
				.ToObservable();

			AllowedGenres = allGenres.Where(x => !Settings.BlacklistedGenreIds.Contains(x.Id)
												 && (string.IsNullOrEmpty(AllowedGenresFilter) || x.Name.ToLower().Contains(AllowedGenresFilter.ToLower())))
				.OrderBy(x => x.Name)
				.ToObservable();
		}
	}
}
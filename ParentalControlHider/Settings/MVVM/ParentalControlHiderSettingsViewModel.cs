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
		private readonly ParentalControlHider _plugin;

		private ParentalControlHiderSettings _editingClone;
		private ParentalControlHiderSettings _settings;
		private ObservableCollection<Tag> _allowedTags;
		private ObservableCollection<Tag> _blacklistedTags;
		private Tag _selectedAllowedTag;
		private Tag _selectedBlacklistedTag;

		public ParentalControlHiderSettingsViewModel(ParentalControlHider plugin)
		{
			_plugin = plugin;
			var savedSettings = plugin.LoadPluginSettings<ParentalControlHiderSettings>();

			Settings = savedSettings ?? new ParentalControlHiderSettings();
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

		public ICommand BlacklistTagCommand => new RelayCommand(() =>
		{
			if (SelectedAllowedTag != null)
			{
				Settings.BlacklistedTagIds.Add(SelectedAllowedTag.Id);
				AllowedTags.Remove(SelectedAllowedTag);
				BlacklistedTags.Add(SelectedAllowedTag);
				SelectedAllowedTag = null;
			}
		});

		public ICommand AllowTagCommand => new RelayCommand(() =>
		{
			if (SelectedBlacklistedTag != null)
			{
				Settings.BlacklistedTagIds.Remove(SelectedBlacklistedTag.Id);
				AllowedTags.Add(SelectedBlacklistedTag);
				BlacklistedTags.Remove(SelectedBlacklistedTag);
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

		private void InitializeTags()
		{
			var allTags = _plugin.PlayniteApi.Database.Tags.ToList();
			BlacklistedTags = allTags.Where(x => Settings.BlacklistedTagIds.Contains(x.Id)).ToObservable();
			AllowedTags = allTags.Except(BlacklistedTags).ToObservable();
		}
	}
}
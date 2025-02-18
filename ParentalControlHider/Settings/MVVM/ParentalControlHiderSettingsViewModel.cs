using System.Collections.Generic;
using Playnite.SDK;
using Playnite.SDK.Data;

namespace ParentalControlHider.Settings.MVVM
{
	public class ParentalControlHiderSettingsViewModel : ObservableObject, ISettings
	{
		private readonly ParentalControlHider _plugin;
		private ParentalControlHiderSettings EditingClone { get; set; }

		private ParentalControlHiderSettings _settings;
		public ParentalControlHiderSettings Settings
		{
			get => _settings;
			set
			{
				_settings = value;
				OnPropertyChanged();
			}
		}

		public ParentalControlHiderSettingsViewModel(ParentalControlHider plugin)
		{
			// Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
			this._plugin = plugin;

			// Load saved settings.
			var savedSettings = plugin.LoadPluginSettings<ParentalControlHiderSettings>();

			// LoadPluginSettings returns null if no saved data is available.
			if (savedSettings != null)
			{
				Settings = savedSettings;
			}
			else
			{
				Settings = new ParentalControlHiderSettings();
			}
		}

		public void BeginEdit()
		{
			// Code executed when settings view is opened and user starts editing values.
			EditingClone = Serialization.GetClone(Settings);
		}

		public void CancelEdit()
		{
			// Code executed when user decides to cancel any changes made since BeginEdit was called.
			// This method should revert any changes made to Option1 and Option2.
			Settings = EditingClone;
		}

		public void EndEdit()
		{
			// Code executed when user decides to confirm changes made since BeginEdit was called.
			// This method should save settings made to Option1 and Option2.
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
	}
}
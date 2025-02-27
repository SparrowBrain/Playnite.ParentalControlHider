using ParentalControlHider.Services;
using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings;
using ParentalControlHider.Settings.MVVM;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParentalControlHider
{
	public class ParentalControlHider : GenericPlugin
	{
		private static readonly ILogger Logger = LogManager.GetLogger();

		private readonly ParentalControlHiderSettingsViewModel _settings;

		public override Guid Id { get; } = Guid.Parse("134725de-cfcb-4474-849b-5d9c52babb75");

		public ParentalControlHider(IPlayniteAPI api) : base(api)
		{
			_settings = new ParentalControlHiderSettingsViewModel(this);
			Properties = new GenericPluginProperties
			{
				HasSettings = true
			};
		}

		public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
		{
			HideGames();
		}

		public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
		{
			HideGames();
		}

		public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
		{
			yield return new MainMenuItem
			{
				Description = ResourceProvider.GetString("LOC_ParentalControlHider_MainMenu_Hide"),
				MenuSection = "@Parental Control Hider",
				Action = actionArgs => { HideGames(); }
			};
			yield return new MainMenuItem
			{
				Description = ResourceProvider.GetString("LOC_ParentalControlHider_MainMenu_UnHide"),
				MenuSection = "@Parental Control Hider",
				Action = actionArgs => { UnHideGames(); }
			};
		}

		public override ISettings GetSettings(bool firstRunSettings)
		{
			return _settings;
		}

		public override UserControl GetSettingsView(bool firstRunSettings)
		{
			return new ParentalControlHiderSettingsView();
		}

		private void HideGames()
		{
			Task.Run(() =>
			{
				try
				{
					var pluginSettingsPersistence = new PluginSettingsPersistence(this);
					var parentalHiderTagProvider = new ParentalHiderTagProvider(PlayniteApi);
					var managedGamesFilter = new ManagedGamesFilter();
					var tagsBlacklist = new TagsBlacklist();
					var mainService = new MainService(
						PlayniteApi,
						pluginSettingsPersistence,
						parentalHiderTagProvider,
						managedGamesFilter,
						tagsBlacklist);

					mainService.HideGames();
				}
				catch (Exception e)
				{
					Logger.Error(e, "Failed to hide games.");
					PlayniteApi.MainView.UIDispatcher.Invoke(() =>
						PlayniteApi.Dialogs.ShowErrorMessage(
							ResourceProvider.GetString("LOC_ParentalControlHider_Error_FailedToHide")));
				}
			});
		}

		private void UnHideGames()
		{
			Task.Run(() =>
			{
				try
				{
					var pluginSettingsPersistence = new PluginSettingsPersistence(this);
					var parentalHiderTagProvider = new ParentalHiderTagProvider(PlayniteApi);
					var managedGamesFilter = new ManagedGamesFilter();
					var tagsBlacklist = new TagsBlacklist();
					var mainService = new MainService(
						PlayniteApi,
						pluginSettingsPersistence,
						parentalHiderTagProvider,
						managedGamesFilter,
						tagsBlacklist);

					mainService.UnHideGames();
				}
				catch (Exception e)
				{
					Logger.Error(e, "Failed to unhide games.");
					PlayniteApi.MainView.UIDispatcher.Invoke(() =>
						PlayniteApi.Dialogs.ShowErrorMessage(
							ResourceProvider.GetString("LOC_ParentalControlHider_Error_FailedToUnHide")));
				}
			});
		}
	}
}
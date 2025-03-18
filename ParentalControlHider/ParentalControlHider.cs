using ParentalControlHider.Infrastructure;
using ParentalControlHider.Services;
using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings.MVVM;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;

namespace ParentalControlHider
{
	public class ParentalControlHider : GenericPlugin
	{
		private static readonly ILogger Logger = LogManager.GetLogger();
		private readonly Timer _hideGamesTimer = new Timer();
		private ParentalControlHiderSettingsViewModel _settings;

		public override Guid Id { get; } = Guid.Parse("134725de-cfcb-4474-849b-5d9c52babb75");

		public ParentalControlHider(IPlayniteAPI api) : base(api)
		{
			Properties = new GenericPluginProperties
			{
				HasSettings = true
			};

			_hideGamesTimer.Elapsed += (sender, args) => { HideGames(); };
		}

		public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
		{
			if (GetExtensionSettings().Settings.RunOnApplicationStarted)
			{
				HideGames();
			}
		}

		public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
		{
			if (GetExtensionSettings().Settings.RunOnLibraryUpdated)
			{
				HideGames();
			}
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
				Description = ResourceProvider.GetString("LOC_ParentalControlHider_MainMenu_Unhide"),
				MenuSection = "@Parental Control Hider",
				Action = actionArgs => { UnhideGames(); }
			};
		}

		public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
		{
			yield return new GameMenuItem
			{
				Description = ResourceProvider.GetString("LOC_ParentalControlHider_GameMenu_AddToWhitelist"),
				Action = actionArgs => { AddGamesToWhitelist(actionArgs.Games); },
				MenuSection = "Parental Control Hider"
			};
			yield return new GameMenuItem
			{
				Description = ResourceProvider.GetString("LOC_ParentalControlHider_GameMenu_RemoveFromWhitelist"),
				Action = actionArgs => { RemoveGamesFromWhitelist(actionArgs.Games); },
				MenuSection = "Parental Control Hider"
			};
		}

		public override ISettings GetSettings(bool firstRunSettings)
		{
			return _settings ?? (_settings = new ParentalControlHiderSettingsViewModel(this, new AgeRatingsAgeProvider()));
		}

		public override UserControl GetSettingsView(bool firstRunSettings)
		{
			return new ParentalControlHiderSettingsView();
		}

		private ParentalControlHiderSettingsViewModel GetExtensionSettings() => GetSettings(false) as ParentalControlHiderSettingsViewModel;

		private void HideGames()
		{
			_hideGamesTimer.Enabled = false;
			_hideGamesTimer.Stop();
			Task.Run(async () =>
			{
				try
				{
					var settings = GetExtensionSettings().Settings;
					var mainService = CreateMainService();
					await mainService.HideGames(settings);
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

		private void UnhideGames()
		{
			Task.Run(async () =>
			{
				try
				{
					var mainService = CreateMainService();
					await mainService.UnhideGames();
					var settings = GetExtensionSettings().Settings;
					if (settings.RunAfterUnhidden)
					{
						_hideGamesTimer.Interval = TimeSpan.FromMinutes(settings.MinutesToRunAfterUnhidden).TotalMilliseconds;
						_hideGamesTimer.Enabled = true;
						_hideGamesTimer.Start();
					}
				}
				catch (Exception e)
				{
					Logger.Error(e, "Failed to Unhide games.");
					PlayniteApi.MainView.UIDispatcher.Invoke(() =>
						PlayniteApi.Dialogs.ShowErrorMessage(
							ResourceProvider.GetString("LOC_ParentalControlHider_Error_FailedToUnhide")));
				}
			});
		}

		private MainService CreateMainService()
		{
			var parentalHiderTagProvider = new ParentalHiderTagProvider(PlayniteApi);
			var managedGamesFilter = new ManagedGamesFilter();
			var dateTimeProvider = new DateTimeProvider();
			var ageRatingsFilter = new AgeRatingsFilter(dateTimeProvider);
			var tagsBlacklist = new TagsBlacklist();
			var genresBlacklist = new GenresBlacklist();
			var gamesToHideFilter = new GamesToHideFilter(ageRatingsFilter, tagsBlacklist, genresBlacklist);
			var gamesWhitelist = new GamesWhitelist();
			var mainService = new MainService(
				PlayniteApi,
				parentalHiderTagProvider,
				managedGamesFilter,
				gamesToHideFilter,
				gamesWhitelist);
			return mainService;
		}

		private void AddGamesToWhitelist(List<Game> games)
		{
			var settingsViewModel = GetExtensionSettings();
			foreach (var game in games)
			{
				settingsViewModel.Settings.WhitelistedGameIds.Add(game.Id);
			}

			SavePluginSettings(settingsViewModel.Settings);
			settingsViewModel.InitializeGames();
		}

		private void RemoveGamesFromWhitelist(List<Game> games)
		{
			var settingsViewModel = GetExtensionSettings();
			foreach (var game in games)
			{
				if (settingsViewModel.Settings.WhitelistedGameIds.Contains(game.Id))
				{
					settingsViewModel.Settings.WhitelistedGameIds.Remove(game.Id);
				}
			}

			SavePluginSettings(settingsViewModel.Settings);
			settingsViewModel.InitializeGames();
		}
	}
}
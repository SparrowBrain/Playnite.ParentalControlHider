using ParentalControlHider.Infrastructure;
using ParentalControlHider.Services;
using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings;
using ParentalControlHider.Settings.MVVM;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
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
			_settings = new ParentalControlHiderSettingsViewModel(this, new AgeRatingsAgeProvider());
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
					var mainService = CreateMainService();
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
					var mainService = CreateMainService();
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

		private MainService CreateMainService()
		{
			var pluginSettingsPersistence = new PluginSettingsPersistence(this);
			var parentalHiderTagProvider = new ParentalHiderTagProvider(PlayniteApi);
			var managedGamesFilter = new ManagedGamesFilter();
			var dateTimeProvider = new DateTimeProvider();
			var ageRatingsFilter = new AgeRatingsFilter(dateTimeProvider);
			var tagsBlacklist = new TagsBlacklist();
			var gamesToHideFilter = new GamesToHideFilter(ageRatingsFilter, tagsBlacklist);
			var gamesWhitelist = new GamesWhitelist();
			var mainService = new MainService(
				PlayniteApi,
				pluginSettingsPersistence,
				parentalHiderTagProvider,
				managedGamesFilter,
				gamesToHideFilter,
				gamesWhitelist);
			return mainService;
		}

		private void AddGamesToWhitelist(List<Game> games)
		{
			foreach (var game in games)
			{
				_settings.Settings.GameWhitelist.Add(game.Id);
			}

			SavePluginSettings(_settings.Settings);
		}

		private void RemoveGamesFromWhitelist(List<Game> games)
		{
			foreach (var game in games)
			{
				if (_settings.Settings.GameWhitelist.Contains(game.Id))
				{
					_settings.Settings.GameWhitelist.Remove(game.Id);
				}
			}

			SavePluginSettings(_settings.Settings);
		}
	}
}
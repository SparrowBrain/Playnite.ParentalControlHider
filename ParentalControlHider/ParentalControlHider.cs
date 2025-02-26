﻿using ParentalControlHider.Services;
using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings;
using ParentalControlHider.Settings.MVVM;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
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

		public override void OnGameInstalled(OnGameInstalledEventArgs args)
		{
			// Add code to be executed when game is finished installing.
		}

		public override void OnGameStarted(OnGameStartedEventArgs args)
		{
			// Add code to be executed when game is started running.
		}

		public override void OnGameStarting(OnGameStartingEventArgs args)
		{
			// Add code to be executed when game is preparing to be started.
		}

		public override void OnGameStopped(OnGameStoppedEventArgs args)
		{
			// Add code to be executed when game is preparing to be started.
		}

		public override void OnGameUninstalled(OnGameUninstalledEventArgs args)
		{
			// Add code to be executed when game is uninstalled.
		}

		public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
		{
			HideGames();
		}

		public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
		{
			// Add code to be executed when Playnite is shutting down.
		}

		public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
		{
			// Add code to be executed when library is updated.
		}

		public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
		{
			yield return new MainMenuItem
			{
				Description = ResourceProvider.GetString("LOC_ParentalControlHider_MainMenu_Hide"),
				MenuSection = "@Parental Control Hider",
				Action = actionArgs => { HideGames(); }
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
	}
}
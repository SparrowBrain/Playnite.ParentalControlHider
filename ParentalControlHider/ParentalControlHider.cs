using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Linq;
using System.Windows.Controls;
using ParentalControlHider.Settings.MVVM;

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
			var tag = PlayniteApi.Database.Tags.FirstOrDefault(x => x.Name == "[ParentalControlHider] Hidden");
			if (tag == null)
			{
				tag = new Tag() { Id = Guid.NewGuid(), Name = "[ParentalControlHider] Hidden" };
				PlayniteApi.Database.Tags.Add(tag);
			}

			var games = PlayniteApi.Database.Games.Where(x => !x.Hidden).ToList();

			using (var update = PlayniteApi.Database.BufferedUpdate())
			{
				foreach (var game in games.Where(x => x.Tags?.Exists(t => t.Name == "Horror") ?? false))
				{
					game.Hidden = true;
					if (!game.TagIds.Contains(tag.Id))
					{
						game.TagIds.Add(tag.Id);
					}

					PlayniteApi.Database.Games.Update(game);
				}
			}
		}

		public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
		{
			// Add code to be executed when Playnite is shutting down.
		}

		public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
		{
			// Add code to be executed when library is updated.
		}

		public override ISettings GetSettings(bool firstRunSettings)
		{
			return _settings;
		}

		public override UserControl GetSettingsView(bool firstRunSettings)
		{
			return new ParentalControlHiderSettingsView();
		}
	}
}
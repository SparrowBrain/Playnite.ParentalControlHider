using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings;
using Playnite.SDK;
using System;
using System.Collections.Generic;

namespace ParentalControlHider.Services
{
	public class MainService
	{
		private readonly IPlayniteAPI _api;
		private readonly IPluginSettingsPersistence _pluginSettingsPersistence;
		private readonly IParentalHiderTagProvider _parentalHiderTagProvider;
		private readonly IManagedGamesFilter _managedGamesFilter;
		private readonly ITagsBlacklist _tagsBlacklist;

		public MainService(
			IPlayniteAPI api,
			IPluginSettingsPersistence pluginSettingsPersistence,
			IParentalHiderTagProvider parentalHiderTagProvider,
			IManagedGamesFilter managedGamesFilter,
			ITagsBlacklist tagsBlacklist)
		{
			_api = api;
			_pluginSettingsPersistence = pluginSettingsPersistence;
			_parentalHiderTagProvider = parentalHiderTagProvider;
			_managedGamesFilter = managedGamesFilter;
			_tagsBlacklist = tagsBlacklist;
		}

		public void HideGames()
		{
			var tag = _parentalHiderTagProvider.GetParentalHiderTag();
			var settings = _pluginSettingsPersistence.LoadPluginSettings<ParentalControlHiderSettings>();

			using (var _ = _api.Database.BufferedUpdate())
			{
				foreach (var game in _api.Database.Games)
				{
					var isHidden = _managedGamesFilter.IsGameManagedByParentalHider(game, tag)
								   && _tagsBlacklist.DoesItContainBlacklistedTag(game, settings);
					game.Hidden = isHidden;

					if (isHidden && !(game.TagIds?.Contains(tag.Id) ?? false))
					{
						if (game.TagIds == null)
						{
							game.TagIds = new List<Guid>();
						}

						game.TagIds.Add(tag.Id);
					}
					else if (!isHidden && (game.TagIds?.Contains(tag.Id) ?? false))
					{
						game.TagIds.Remove(tag.Id);
					}

					_api.Database.Games.Update(game);
				}
			}
		}

		public void UnHideGames()
		{
			var tag = _parentalHiderTagProvider.GetParentalHiderTag();

			using (var _ = _api.Database.BufferedUpdate())
			{
				foreach (var game in _api.Database.Games)
				{
					if (game.Hidden && (game.TagIds?.Contains(tag.Id) ?? false))
					{
						game.Hidden = false;
						game.TagIds.Remove(tag.Id);
						_api.Database.Games.Update(game);
					}
				}
			}
		}
	}
}
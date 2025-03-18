using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings;
using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParentalControlHider.Services
{
	public class MainService
	{
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

		private readonly IPlayniteAPI _api;
		private readonly IParentalHiderTagProvider _parentalHiderTagProvider;
		private readonly IManagedGamesFilter _managedGamesFilter;
		private readonly IGamesToHideFilter _gamesToHideFilter;
		private readonly IGamesWhitelist _gamesWhitelist;

		public MainService(
			IPlayniteAPI api,
			IParentalHiderTagProvider parentalHiderTagProvider,
			IManagedGamesFilter managedGamesFilter,
			IGamesToHideFilter gamesToHideFilter,
			IGamesWhitelist gamesWhitelist)
		{
			_api = api;
			_parentalHiderTagProvider = parentalHiderTagProvider;
			_managedGamesFilter = managedGamesFilter;
			_gamesToHideFilter = gamesToHideFilter;
			_gamesWhitelist = gamesWhitelist;
		}

		public async Task HideGames(ParentalControlHiderSettings settings)
		{
			await _semaphore.WaitAsync();
			var tag = _parentalHiderTagProvider.GetParentalHiderTag();

			using (var _ = _api.Database.BufferedUpdate())
			{
				Parallel.ForEach(_api.Database.Games, game =>
				{
					if (!_managedGamesFilter.IsGameManagedByParentalHider(game, tag))
					{
						return;
					}

					var isHidden = _gamesToHideFilter.ShouldHideTheGame(game, settings) && !_gamesWhitelist.IsOnWhitelist(game, settings);
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
				});
			}
		}

		public async Task UnhideGames()
		{
			await _semaphore.WaitAsync();
			var tag = _parentalHiderTagProvider.GetParentalHiderTag();

			using (var _ = _api.Database.BufferedUpdate())
			{
				Parallel.ForEach(_api.Database.Games, game =>
				{
					if (game.Hidden && (game.TagIds?.Contains(tag.Id) ?? false))
					{
						game.Hidden = false;
						game.TagIds.Remove(tag.Id);
						_api.Database.Games.Update(game);
					}
				});
			}
		}
	}
}
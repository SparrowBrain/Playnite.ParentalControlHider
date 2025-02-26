using AutoFixture.Xunit2;
using FakeItEasy;
using ParentalControlHider.Services;
using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings;
using Playnite.SDK;
using Playnite.SDK.Models;
using System.Collections.Generic;
using TestTools.Shared;
using Xunit;

namespace ParentalControlHider.UnitTests.Services
{
	public class MainServiceTests
	{
		[Theory]
		[AutoFakeItEasyData]
		public void HideGames_(
			[Frozen] IPlayniteAPI api,
			[Frozen] IParentalHiderTagProvider parentalHiderTagProvider,
			[Frozen] IManagedGamesFilter managedGamesFilter,
			[Frozen] ITagsBlacklist tagsBlacklist,
			List<Game> games,
			Tag parentalHiderTag,
			MainService sut)
		{
			// Arrange
			var testableGames = new TestableItemCollection<Game>(games);
			A.CallTo(() => api.Database.Games).Returns(testableGames);
			A.CallTo(() => parentalHiderTagProvider.GetParentalHiderTag()).Returns(parentalHiderTag);
			A.CallTo(() => managedGamesFilter.IsGameManagedByParentalHider(A<Game>._, A<Tag>._)).Returns(true);
			A.CallTo(() => tagsBlacklist.DoesItContainBlacklistedTag(A<Game>._, A<ParentalControlHiderSettings>._)).Returns(false);

			// Act
			sut.HideGames();

			// Assert
			Assert.All(testableGames, x => Assert.False(x.Hidden));
		}
	}

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

					if (isHidden && !game.TagIds.Contains(tag.Id))
					{
						game.TagIds.Add(tag.Id);
					}
					else if (!isHidden && game.TagIds.Contains(tag.Id))
					{
						game.TagIds.Remove(tag.Id);
					}

					_api.Database.Games.Update(game);
				}
			}
		}
	}
}
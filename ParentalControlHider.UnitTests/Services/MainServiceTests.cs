using AutoFixture.Xunit2;
using FakeItEasy;
using ParentalControlHider.Services;
using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings;
using Playnite.SDK;
using Playnite.SDK.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTools.Shared;
using Xunit;

namespace ParentalControlHider.UnitTests.Services
{
	public class MainServiceTests
	{
		[Theory]
		[AutoFakeItEasyData]
		public async Task HideGames_(
			[Frozen] IPlayniteAPI api,
			[Frozen] IParentalHiderTagProvider parentalHiderTagProvider,
			[Frozen] IManagedGamesFilter managedGamesFilter,
			[Frozen] ITagsBlacklist tagsBlacklist,
			ParentalControlHiderSettings settings,
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
			await sut.HideGames(settings);

			// Assert
			Assert.All(testableGames, x => Assert.False(x.Hidden));
		}
	}
}
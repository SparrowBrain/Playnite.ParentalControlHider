using ParentalControlHider.Services.Filters;
using Playnite.SDK.Models;
using TestTools.Shared;
using Xunit;

namespace ParentalControlHider.UnitTests.Services.Filters
{
	public class ManagedGamesFilterTests
	{
		[Theory]
		[AutoFakeItEasyData]
		public void IsGameManagedByParentalHider_ReturnsTrue_WhenGameIsNotHidden(
			Tag parentalHiderTag,
			Game game,
			ManagedGamesFilter sut)
		{
			// Arrange
			game.Hidden = false;

			// Act
			var actual = sut.IsGameManagedByParentalHider(game, parentalHiderTag);

			// Assert
			Assert.True(actual);
		}

		[Theory]
		[AutoFakeItEasyData]
		public void IsGameManagedByParentalHider_ReturnsTrue_WhenGameIsHiddenAndHasParentalHiderTag(
			Tag parentalHiderTag,
			Game game,
			ManagedGamesFilter sut)
		{
			// Arrange
			game.Hidden = true;
			game.TagIds.Add(parentalHiderTag.Id);

			// Act
			var actual = sut.IsGameManagedByParentalHider(game, parentalHiderTag);

			// Assert
			Assert.True(actual);
		}

		[Theory]
		[AutoFakeItEasyData]
		public void IsGameManagedByParentalHider_ReturnsFalse_WhenGameIsHiddenAndDoesNotHaveParentalHiderTag(
			Tag parentalHiderTag,
			Game game,
			ManagedGamesFilter sut)
		{
			// Arrange
			game.Hidden = true;

			// Act
			var actual = sut.IsGameManagedByParentalHider(game, parentalHiderTag);

			// Assert
			Assert.False(actual);
		}
	}
}
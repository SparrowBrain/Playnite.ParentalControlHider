using AutoFixture.Xunit2;
using FakeItEasy;
using ParentalControlHider.Settings;
using Playnite.SDK.Models;
using System.Linq;
using ParentalControlHider.Infrastructure;
using ParentalControlHider.Services.Filters;
using TestTools.Shared;
using Xunit;

namespace ParentalControlHider.UnitTests.Services.Filters
{
	public class AgeRatingsFilterTests
	{
		[Theory]
		[AutoFakeItEasyData]
		public void IsAgeAllowed_ReturnsTrue_WhenAgeIsNotRestricted(
			Game game,
			ParentalControlHiderSettings settings,
			AgeRatingsFilter sut)
		{
			// Act
			var actual = sut.IsAgeAllowed(game, settings);

			// Assert
			Assert.True(actual);
		}

		[Theory]
		[AutoFakeItEasyData]
		public void IsAgeAllowed_ReturnsTrue_WhenAgeIsRestrictedButTheKidIsOlder(
			[Frozen] IDateTimeProvider dateTimeProvider,
			int kidAge,
			Game game,
			ParentalControlHiderSettings settings,
			AgeRatingsFilter sut)
		{
			// Arrange
			var usedAgeRating = settings.AgeRatingsWithAge.Last();
			game.AgeRatingIds.Add(usedAgeRating.Key);
			settings.AgeRatingsWithAge[usedAgeRating.Key] = kidAge;
			settings.UsedAgeRatings.Add(usedAgeRating.Key);
			A.CallTo(() => dateTimeProvider.Now).Returns(settings.Birthday.AddYears(kidAge));

			// Act
			var actual = sut.IsAgeAllowed(game, settings);

			// Assert
			Assert.True(actual);
		}

		[Theory]
		[AutoFakeItEasyData]
		public void IsAgeAllowed_ReturnsFalse_WhenAgeIsRestrictedAndKidIsYounger(
			[Frozen] IDateTimeProvider dateTimeProvider,
			int kidAge,
			Game game,
			ParentalControlHiderSettings settings,
			AgeRatingsFilter sut)
		{
			// Arrange
			var usedAgeRating = settings.AgeRatingsWithAge.Last();
			game.AgeRatingIds.Add(usedAgeRating.Key);
			settings.AgeRatingsWithAge[usedAgeRating.Key] = kidAge + 1;
			settings.UsedAgeRatings.Add(usedAgeRating.Key);
			A.CallTo(() => dateTimeProvider.Now).Returns(settings.Birthday.AddYears(kidAge));

			// Act
			var actual = sut.IsAgeAllowed(game, settings);

			// Assert
			Assert.False(actual);
		}
	}
}
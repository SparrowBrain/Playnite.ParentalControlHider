using System.Linq;
using ParentalControlHider.Services.Filters;
using ParentalControlHider.Settings;
using Playnite.SDK.Models;
using TestTools.Shared;
using Xunit;

namespace ParentalControlHider.UnitTests.Services.Filters
{
	public class TagsBlacklistTests
	{
		[Theory]
		[AutoFakeItEasyData]
		public void DoesItContainBlacklistedTag_ReturnsFalse_WhenGameDoesNotHaveBlacklistedTags(
			Game game,
			ParentalControlHiderSettings settings,
			TagsBlacklist sut)
		{
			// Act
			var actual = sut.DoesItContainBlacklistedTag(game, settings);

			// Assert
			Assert.False(actual);
		}

		[Theory]
		[AutoFakeItEasyData]
		public void DoesItContainBlacklistedTag_ReturnsTrue_WhenGameHasBlacklistedTag(
			Game game,
			ParentalControlHiderSettings settings,
			TagsBlacklist sut)
		{
			// Act
			var blackistedTag = settings.BlacklistedTagIds.Last();
			game.TagIds.Add(blackistedTag);
			var actual = sut.DoesItContainBlacklistedTag(game, settings);

			// Assert
			Assert.True(actual);
		}
	}
}
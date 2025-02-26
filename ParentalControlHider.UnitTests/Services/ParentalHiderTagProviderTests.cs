using AutoFixture.Xunit2;
using FakeItEasy;
using ParentalControlHider.Services;
using Playnite.SDK;
using Playnite.SDK.Models;
using System.Collections.Generic;
using System.Linq;
using TestTools.Shared;
using Xunit;

namespace ParentalControlHider.UnitTests.Services
{
	public class ParentalHiderTagProviderTests
	{
		private const string TagName = "[ParentalControlHider] Hidden";

		[Theory]
		[AutoFakeItEasyData]
		public void GetParentalHiderTag_CreatesTag_WhenTagDoesNotExist(
			[Frozen] IPlayniteAPI api,
			List<Tag> tags,
			ParentalHiderTagProvider sut)
		{
			// Arrange
			var testableTags = new TestableItemCollection<Tag>(tags);
			A.CallTo(() => api.Database.Tags).Returns(testableTags);

			// Act
			var actual = sut.GetParentalHiderTag();

			// Assert
			Assert.NotNull(actual);
			Assert.Equal(TagName, actual.Name);
			Assert.Contains(actual, testableTags);
		}

		[Theory]
		[AutoFakeItEasyData]
		public void GetParentalHiderTag_ReturnsTag_WhenTagExists(
			[Frozen] IPlayniteAPI api,
			List<Tag> tags,
			ParentalHiderTagProvider sut)
		{
			// Arrange
			var ourTag = tags.Last();
			ourTag.Name = TagName;
			A.CallTo(() => api.Database.Tags).Returns(new TestableItemCollection<Tag>(tags));

			// Act
			var actual = sut.GetParentalHiderTag();

			// Assert
			Assert.NotNull(actual);
			Assert.Equivalent(ourTag, actual);
		}
	}
}
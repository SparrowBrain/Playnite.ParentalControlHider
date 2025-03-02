using AutoFixture.Xunit2;
using ParentalControlHider.Services;
using Xunit;

namespace ParentalControlHider.UnitTests.Services
{
	public class AgeRatingsAgeProviderTests
	{
		[Theory]
		[InlineAutoData("ESRB EC", 0)]
		[InlineAutoData("ESRB E", 0)]
		[InlineAutoData("ESRB E10", 10)]
		[InlineAutoData("ESRB T", 13)]
		[InlineAutoData("ESRB M", 17)]
		[InlineAutoData("ESRB AO", 18)]
		[InlineAutoData("PEGI 3", 3)]
		[InlineAutoData("PEGI 7", 7)]
		[InlineAutoData("PEGI 12", 12)]
		[InlineAutoData("PEGI 16", 16)]
		[InlineAutoData("PEGI 18", 18)]
		public void GetAgeFromAgeRating(string ageRating, int expected, AgeRatingsAgeProvider sut)
		{
			// Act
			var result = sut.GetAge(ageRating);

			// Assert
			Assert.Equal(expected, result);
		}

		[Theory]
		[AutoData]
		public void GetAge_ReturnsZero_WhenNonConfiguredAge(string ageRating, AgeRatingsAgeProvider sut)
		{
			// Act
			var result = sut.GetAge(ageRating);

			// Assert
			Assert.Equal(0, result);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Xunit;

namespace ParentalControlHider.UnitTests.Services
{
	public class AgeRatingsAgeProviderTests
	{
		[Theory]
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
	}

	public class AgeRatingsAgeProvider
	{
		public int GetAge(string ageRating)
		{
			throw new NotImplementedException();
		}
	}
}
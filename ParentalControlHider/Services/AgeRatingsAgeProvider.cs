using System.Collections.Generic;

namespace ParentalControlHider.Services
{
	public class AgeRatingsAgeProvider : IAgeRatingsAgeProvider
	{
		private Dictionary<string, int> _ageRatings = new Dictionary<string, int>
		{
			{ "PEGI 3", 3 },
			{ "PEGI 7", 7 },
			{ "PEGI 12", 12 },
			{ "PEGI 16", 16 },
			{ "PEGI 18", 18 },
			{ "ESRB EC", 0 },
			{ "ESRB E", 0 },
			{ "ESRB E10", 10 },
			{ "ESRB T", 13 },
			{ "ESRB M", 17 },
			{ "ESRB AO", 18 }
		};

		public int GetAge(string ageRating)
		{
			if (_ageRatings.TryGetValue(ageRating, out var age))
			{
				return age;
			}

			return 0;
		}
	}
}
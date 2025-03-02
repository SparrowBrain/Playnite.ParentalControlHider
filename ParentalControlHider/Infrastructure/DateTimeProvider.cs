using System;

namespace ParentalControlHider.Infrastructure
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime Now => DateTime.Now;
	}
}
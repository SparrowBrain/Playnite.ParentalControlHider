using System;

namespace ParentalControlHider.Infrastructure
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }
	}
}
using System;
using System.Windows.Markup;

namespace ParentalControlHider.Infrastructure.Converters
{
	public abstract class BaseConverter : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace ParentalControlHider.Infrastructure.Converters
{
	internal class BooleanToCollapsedVisibilityConverter : BaseConverter, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isTrue && isTrue)
			{
				return Visibility.Visible;
			}

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

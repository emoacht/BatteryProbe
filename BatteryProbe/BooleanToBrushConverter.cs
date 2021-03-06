﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BatteryProbe
{
	[ValueConversion(typeof(Boolean), typeof(Brush))]
	public class BooleanToBrushConverter : IValueConverter
	{
		public Brush TailBrush { get; set; } = Brushes.Black;
		public Brush HeadBrush { get; set; } = Brushes.White;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool source))
				return DependencyProperty.UnsetValue;

			return source ? HeadBrush : TailBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
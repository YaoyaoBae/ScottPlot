﻿using System;
using System.Globalization;

namespace ScottPlot.Config.DateTimeTickUnits
{
    public class DateTimeTickHundredYear : DateTimeTickYear
    {
        public DateTimeTickHundredYear(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnitKind.HundredYear;
            if (manualSpacing == null)
                deltas = new int[] { 1, 2, 5 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year - (value.Year % 100), 1, 1);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddYears(delta * 100);
        }

        protected override string GetTickLabel(DateTime value)
        {
            var dt = new DateTime(value.Year, 1, 1);
            string localizedLabel = dt.ToString("yyyy", culture); // year only
            return localizedLabel + "\n ";
        }
    }
}

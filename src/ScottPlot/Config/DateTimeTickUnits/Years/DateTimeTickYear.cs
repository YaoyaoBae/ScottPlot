﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace ScottPlot.Config.DateTimeTickUnits
{
    public class DateTimeTickYear : DateTimeTickUnitBase
    {
        public DateTimeTickYear(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnitKind.Year;
            if (manualSpacing != null)
                throw new NotImplementedException("can't display years with fixed spacing (use numeric axis instead)");
            deltas = new int[] { 1, 2, 5 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year, 1, 1);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddYears(delta);
        }

        protected override string GetTickLabel(DateTime value)
        {
            var dt = new DateTime(value.Year, 1, 1);
            string localizedLabel = dt.ToString("yyyy", culture); // year only
            return localizedLabel + "\n ";
        }

        protected override DateTime[] GetTicks(DateTime from, DateTime to, int delta)
        {
            var dates = new List<DateTime>();
            DateTime dt = Floor(from);
            while (dt <= to)
            {
                if (dt >= from)
                    dates.Add(dt);
                try
                {
                    dt = Increment(dt, delta);
                }
                catch
                {
                    break; // our date is larger than possible
                }
            }
            return dates.ToArray();
        }
    }
}

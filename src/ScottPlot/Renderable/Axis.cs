﻿using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// This class holds axis rendering details (label, ticks, tick labels) but no logic
    /// </summary>
    public class Axis : IRenderable
    {
        private Edge _Edge;
        public Edge Edge
        {
            get => _Edge;
            set
            {
                _Edge = value;
                Line.Edge = value;
                Title.Edge = value;
                Ticks.Edge = value;
                Ticks.TickCollection.verticalAxis = (value == Edge.Left || value == Edge.Right);
            }
        }
        public bool IsHorizontal => Edge == Edge.Top || Edge == Edge.Bottom;
        public bool IsVertical => Edge == Edge.Left || Edge == Edge.Right;
        public bool IsVisible { get; set; } = true;

        public float PixelSize = 123;
        public float PixelSizeMinimum = 5;

        public readonly AxisTitle Title = new AxisTitle();
        public readonly AxisTicks Ticks = new AxisTicks();
        public readonly AxisLine Line = new AxisLine();

        public int RecalculationCount { get; private set; } = 0;
        public void RecalculateTickPositions(PlotDimensions dims)
        {
            Ticks.TickCollection.Recalculate(dims, Ticks.MajorLabelFont);
            RecalculationCount += 1;
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (RecalculationCount == 0)
                RecalculateTickPositions(dims);

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            {
                Ticks.Render(dims, bmp, lowQuality);
                Title.Render(dims, bmp, lowQuality);
                Line.Render(dims, bmp, lowQuality);
            }
        }

        // pass arguments from the Plot module to this level
        public void Configure(
            bool? showTitle = null,
            bool? showLabels = null,
            bool? showMajorTicks = null,
            bool? showMinorTicks = null,
            bool? showLine = null,
            Color? color = null,
            bool? useMultiplierNotation = null,
            bool? useOffsetNotation = null,
            bool? useExponentialNotation = null,
            bool? dateTime = null,
            bool? rulerMode = null,
            bool? invertSign = null,
            string fontName = null,
            float? fontSize = null,
            double? rotation = null,
            bool? logScale = null,
            string numericFormatString = null,
            bool? snapToNearestPixel = null,
            int? radix = null,
            string prefix = null,
            string dateTimeFormatString = null
            )
        {
            Title.IsVisible = showTitle ?? Title.IsVisible;
            Ticks.MajorLabelEnable = showLabels ?? Ticks.MajorLabelEnable;
            Ticks.MajorTickEnable = showMajorTicks ?? Ticks.MajorTickEnable;
            Ticks.MinorTickEnable = showMinorTicks ?? Ticks.MinorTickEnable;
            Line.IsVisible = showLine ?? Line.IsVisible;

            Ticks.Color = color ?? Ticks.Color;
            Title.Font.Color = color ?? Title.Font.Color;
            Line.Color = color ?? Line.Color;

            Ticks.TickCollection.useMultiplierNotation = useMultiplierNotation ?? Ticks.TickCollection.useMultiplierNotation;
            Ticks.TickCollection.useOffsetNotation = useOffsetNotation ?? Ticks.TickCollection.useOffsetNotation;
            Ticks.TickCollection.useExponentialNotation = useExponentialNotation ?? Ticks.TickCollection.useExponentialNotation;

            Ticks.TickCollection.dateFormat = dateTime ?? Ticks.TickCollection.dateFormat;
            Ticks.RulerMode = rulerMode ?? Ticks.RulerMode;
            Ticks.TickCollection.invertSign = invertSign ?? Ticks.TickCollection.invertSign;
            Ticks.MajorLabelFont.Name = fontName ?? Ticks.MajorLabelFont.Name;
            Ticks.MajorLabelFont.Size = fontSize ?? Ticks.MajorLabelFont.Size;
            Ticks.Rotation = (float)(rotation ?? Ticks.Rotation);
            Ticks.MinorTickLogDistribution = logScale ?? Ticks.MinorTickLogDistribution;
            Ticks.TickCollection.numericFormatString = numericFormatString ?? Ticks.TickCollection.numericFormatString;
            Ticks.SnapPx = snapToNearestPixel ?? Ticks.SnapPx;
            Ticks.TickCollection.radix = radix ?? Ticks.TickCollection.radix;
            Ticks.TickCollection.prefix = prefix ?? Ticks.TickCollection.prefix;
            Ticks.TickCollection.dateTimeFormatString = dateTimeFormatString ?? Ticks.TickCollection.dateTimeFormatString;
        }

        public void AutoSize()
        {
            using (var tickFont = GDI.Font(Ticks.MajorLabelFont))
            using (var titleFont = GDI.Font(Title.Font))
            {
                PixelSize = 0;

                if (Title.IsVisible)
                    PixelSize += GDI.MeasureString(Title.Label, Title.Font).Height;

                if (Ticks.MajorLabelEnable)
                    PixelSize += IsHorizontal ? Ticks.TickCollection.maxLabelSize.Height : Ticks.TickCollection.maxLabelSize.Width * 1.2f;

                if (Ticks.MajorTickEnable)
                    PixelSize += Ticks.MajorTickLength;

                PixelSize = Math.Max(PixelSize, PixelSizeMinimum);
            }
        }
    }
}
﻿using NUnit.Framework;
using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.AxisRenderTests
{
    class AxisStyling
    {
        private MeanPixel Mean(ScottPlot.Renderable.Axis axis, PlotDimensions dims = null, bool save = true)
        {
            dims = dims ?? new PlotDimensions(
                figureSize: new SizeF(100, 500),
                dataSize: new SizeF(20, 400),
                dataOffset: new PointF(75, 50),
                axisLimits: new AxisLimits2D(-1, 1, -100, 100));

            using (var bmp = new System.Drawing.Bitmap((int)dims.Width, (int)dims.Height))
            using (var gfx = GDI.Graphics(bmp, lowQuality: true))
            using (var brush = GDI.Brush(Color.FromArgb(25, Color.Black)))
            {
                gfx.Clear(Color.White);
                gfx.FillRectangle(brush, dims.DataOffsetX, dims.DataOffsetY, dims.DataWidth, dims.DataHeight);
                axis.Render(dims, bmp);
                if (save)
                    TestTools.SaveFig(bmp);
                return new MeanPixel(bmp);
            }
        }

        [Test]
        public void Test_AxisMajorTick_Enable()
        {
            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Sample Left Axis";
            axis.Edge = ScottPlot.Renderable.Edge.Left;
            axis.Ticks.TickCollection.verticalAxis = true;
            var before = Mean(axis);

            axis.Ticks.MajorTickEnable = false;
            var after = Mean(axis);
            Assert.That(after.IsLighterThan(before));
        }

        [Test]
        public void Test_AxisMinorTick_Enable()
        {
            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Sample Left Axis";
            axis.Edge = ScottPlot.Renderable.Edge.Left;
            axis.Ticks.TickCollection.verticalAxis = true;
            var before = Mean(axis);

            axis.Ticks.MinorTickEnable = false;
            var after = Mean(axis);
            Assert.That(after.IsLighterThan(before));
        }

        [Test]
        public void Test_AxisTitle_Enable()
        {
            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Sample Left Axis";
            axis.Edge = ScottPlot.Renderable.Edge.Left;
            axis.Ticks.TickCollection.verticalAxis = true;
            var before = Mean(axis);

            axis.Title.IsVisible = false;
            var after = Mean(axis);
            Assert.That(after.IsLighterThan(before));
        }

        [Test]
        public void Test_AxisLine_Enable()
        {
            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Sample Left Axis";
            axis.Edge = ScottPlot.Renderable.Edge.Left;
            axis.Ticks.TickCollection.verticalAxis = true;
            var before = Mean(axis);

            axis.Line.IsVisible = false;
            var after = Mean(axis);
            Assert.That(after.IsLighterThan(before));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using BenTools.Mathematics;

namespace MyNotSoLittlePryczkoptron
{
	public class VoronoiDiagramContext
	{
		public int Width { get; set; } = 320;
		public int Height { get; set; } = 320;
		public double MinX { get; set; } = -1.0;
		public double MaxX { get; set; } = 1.0;
		public double MinY { get; set; } = -1.0;
		public double MaxY { get; set; } = 1.0;
		public float[] DataPointsRadii { get; set; } = { 4f, 1f };
		public Color BackgroundColor { get; set; } = Color.Black;
		public Color LineColor { get; set; } = Color.White;
		public Color[] DataPointsColors { get; set; } = { Color.Red, Color.Green };
	}

    public static class VoronoiDiagram
    {
        public static Image CreateImage(VoronoiDiagramContext context, params IEnumerable<Point>[] inputs)
        {
			IEnumerable<IEnumerable<Vector>> dataPoints = inputs.Select(
				input => input.Select(
					point => new Vector(point.XCoordinate, point.YCoordinate)
				)
			);
			double factorX = context.Width / (context.MaxX - context.MinX);
			double factorY = context.Height / (context.MaxY - context.MinY);
			double factor = Math.Min(factorX, factorY);
			double minX = context.MinX - context.Width * (0.5 / factor - 0.5 / factorX);
			double minY = context.MinY - context.Height * (0.5 / factor - 0.5 / factorY);
			dataPoints = dataPoints.Select(points => points.Select(vector => new Vector(
                (vector[0] - minX) * factor,
				(vector[1] - minY) * factor
			)));

            PointF toPoint(Vector vector) => new PointF(
                (float)vector[0],
                (float)vector[1]
            );
            RectangleF toRectangle(Vector vector, float radius) => new RectangleF(
                (float)(vector[0]) - radius,
                (float)(vector[1]) - radius,
				radius * 2f,
				radius * 2f
			);

            VoronoiGraph graph = Fortune.ComputeVoronoiGraph(dataPoints.First().Distinct());
            var image = new Bitmap(context.Width, context.Height);
            var linePen = new Pen(context.LineColor);
			var dataPointBrushes = context.DataPointsColors.Select(color => new SolidBrush(color));
            var surface = Graphics.FromImage(image);
            surface.Clear(context.BackgroundColor);
            surface.SmoothingMode = SmoothingMode.HighQuality;

			double infiniteLength = context.Width + context.Height;
			foreach (var edge in graph.Edges)
            {
                Vector left = edge.VVertexA;
                Vector right = edge.VVertexB;
                if (edge.IsPartlyInfinite)
                {
                    Vector extension = edge.DirectionVector * infiniteLength;
                    if (left == Fortune.VVInfinite)
                    {
                        left = edge.FixedPoint - extension;
                    }
                    if (right == Fortune.VVInfinite)
                    {
                        right = edge.FixedPoint + extension;
                    }
                }
                surface.DrawLine(linePen, toPoint(left), toPoint(right));
            }
			
			var target = dataPoints.Zip(
				dataPointBrushes.Zip(
					context.DataPointsRadii,
					(brush, radius) => new { brush, radius }),
				(points, style) => new { points, style }
			);
			foreach (var data in target)
			{
				foreach (var point in data.points)
				{
					surface.FillEllipse(data.style.brush, toRectangle(point, data.style.radius));
				}
			}

            return image;
        }
    }
}

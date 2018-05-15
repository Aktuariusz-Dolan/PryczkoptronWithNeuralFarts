using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using BenTools.Mathematics;

namespace MyNotSoLittlePryczkoptron
{
    public static class VoronoiDiagram
    {
        public static Image CreateImage(int width, int height, IEnumerable<Point> points, double scale = 1.0, double pointRadius = 4.0)
        {
            IEnumerable<Vector> dataPoints = points.Select(point => new Vector(point.XCoordinate, point.YCoordinate));

            double minX = dataPoints.Min(vertex => vertex[0]);
            double maxX = dataPoints.Max(vertex => vertex[0]);
            double avgX = (minX + maxX) / 2.0;
            double cenX = width / 2;
            double spanX = maxX - minX;

            double minY = dataPoints.Min(vertex => vertex[1]);
            double maxY = dataPoints.Max(vertex => vertex[1]);
            double avgY = (minY + maxY) / 2.0;
            double cenY = height / 2;
            double spanY = maxY - minY;

            double infiniteLength = width + height;

            double factor = scale / Math.Max(spanX / width, spanY / height);
            dataPoints = dataPoints.Select(vector => new Vector(
                (vector[0] - avgX) * factor + cenX,
                (vector[1] - avgY) * factor + cenY
            ));

            PointF toPoint(Vector vector) => new PointF(
                (float)vector[0],
                (float)vector[1]
            );
            float pointDiameter = (float)(pointRadius * 2.0);
            RectangleF toRectangle(Vector vector) => new RectangleF(
                (float)(vector[0] - pointRadius),
                (float)(vector[1] - pointRadius),
                pointDiameter,
                pointDiameter
            );

            VoronoiGraph graph = Fortune.ComputeVoronoiGraph(dataPoints);

            var image = new Bitmap(width, height);
            var linePen = new Pen(Color.White);
            var pointBrush = new SolidBrush(Color.Red);
            var surface = Graphics.FromImage(image);
            surface.Clear(Color.Black);
            surface.SmoothingMode = SmoothingMode.HighQuality;

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

            foreach (var point in dataPoints)
            {
                surface.FillEllipse(pointBrush, toRectangle(point));
            }

            return image;
        }
    }
}

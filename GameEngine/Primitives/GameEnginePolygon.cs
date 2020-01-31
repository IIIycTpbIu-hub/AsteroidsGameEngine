using System;

namespace GameEngine.Primitives
{
    public class GameEnginePolygon
    {
        private float _angleRotate;

        public float AngleRotate
        {
            get { return _angleRotate; }
            private set
            {
                if (value > 360f)
                {
                    value -= 360f;
                }

                if (value < 0)
                {
                    value += 360f;
                }

                _angleRotate = value;
            }
        }

        public Point2D[] Points { get; private set; }

        public Point2D Pivot { get; private set; }

        public GameEnginePolygon(Point2D[] points, Point2D spawnPoint)
        {
            if (points.Length < 3)
            {
                throw new ArgumentException("Array's points value must be more or equals to 3");
            }

            Points = new Point2D[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                Points[i] = new Point2D(points[i].X, points[i].Y);
            }

            Pivot = GetPolygonCenterPoint2D();
            for (int i = 0; i < points.Length; i++)
            {
                Points[i] += spawnPoint - Pivot;
            }

            Pivot = GetPolygonCenterPoint2D();
        }

        public void RotatePolygon(float angleRotate)
        {
            AngleRotate += angleRotate;
            Axes2D axes = new Axes2D(new Point2D(0, 0), angleRotate);
            Point2D rotatedPoint2D;

            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] -= Pivot;
                rotatedPoint2D = axes.X * Points[i].X + axes.Y * Points[i].Y + Pivot;
                Points[i] = rotatedPoint2D;
            }
        }

        public void SetDirectionVector(Point2D directionVector)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] += directionVector;
            }

            Pivot = GetPolygonCenterPoint2D();
        }

        private Point2D GetPolygonCenterPoint2D()
        {
            float centerX = 0;
            float centerY = 0;

            foreach (var point in Points)
            {
                centerX += point.X;
                centerY += point.Y;
            }

            centerX /= Points.Length;
            centerY /= Points.Length;
            return new Point2D(centerX, centerY);
        }
    }
}

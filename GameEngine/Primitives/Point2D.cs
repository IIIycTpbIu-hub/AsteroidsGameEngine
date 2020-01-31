using System;

namespace GameEngine.Primitives
{
    public struct Point2D
    {
        public readonly float X;
        public readonly float Y;

        public Point2D(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point2D operator +(Point2D a, Point2D b)
        {
            return new Point2D(a.X + b.X, a.Y + b.Y);
        }

        public static Point2D operator +(Point2D a, float b)
        {
            return new Point2D(a.X + b, a.Y + b);
        }

        public static Point2D operator -(Point2D a, Point2D b)
        {
            return new Point2D(a.X - b.X, a.Y - b.Y);
        }

        public static Point2D operator -(Point2D a, float b)
        {
            return new Point2D(a.X - b, a.Y - b);
        }

        public static Point2D operator *(Point2D a, Point2D b)
        {
            return new Point2D(a.X * b.X, a.Y * b.Y);
        }

        public static Point2D operator *(Point2D a, float b)
        {
            return new Point2D(a.X * b, a.Y * b);
        }

        public static Point2D operator /(Point2D a, Point2D b)
        {
            return new Point2D(a.X / b.X, a.Y / b.Y);
        }

        public static Point2D operator /(Point2D a, float b)
        {
            return new Point2D(a.X / b, a.Y / b);
        }

        public static Point2D RotatePoint2D(Point2D point, Point2D pivot, float angleRotate)
        {
            double radianAngelRotate = angleRotate * Math.PI / 180;
            var newX = (pivot.X + ((point.X - pivot.X) * Math.Cos(radianAngelRotate))) -
                       ((point.Y - pivot.Y) * Math.Sin(radianAngelRotate));

            var newY = (pivot.Y + ((point.Y - pivot.Y) * Math.Cos(radianAngelRotate))) +
                       ((point.X - pivot.X) * Math.Sin(radianAngelRotate));
            return new Point2D((float)newX, (float)newY);
        }

        public static float Distance(Point2D a, Point2D b)
        {
            return (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public static float VectorDisnace(Point2D a)
        {
            return (float)Math.Sqrt(Math.Pow(a.X, 2) + Math.Pow(a.Y, 2));
        }

        public override string ToString()
        {
            return this.X + " " + this.Y;
        }
    }
}

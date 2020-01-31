namespace GameEngine.Primitives
{
    public class Axes2D
    {
        private readonly Point2D _origin;

        public Point2D X { get; private set; }
        public Point2D Y { get; private set; }

        public Axes2D(Point2D origin, float angleRotate)
        {
            _origin = origin;
            X = new Point2D(_origin.X + 1, _origin.Y);
            Y = new Point2D(_origin.X, _origin.Y + 1);
            RotateAxes2D(angleRotate);
        }

        public void RotateAxes2D(float angleRotate)
        {
            this.X = Point2D.RotatePoint2D(this.X, _origin, angleRotate);
            this.Y = Point2D.RotatePoint2D(this.Y, _origin, angleRotate);
        }
    }
}

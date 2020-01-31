using GameEngine.Primitives;

namespace GameEngine.GameObjects.Enemies
{
    public class Asteroid : Enemy
    {
        public Asteroid(Point2D[] points2D, Point2D creationPoint2D) : base(points2D, creationPoint2D)
        {

        }

        protected internal override void Move()
        {
            Axes2D axes = new Axes2D(new Point2D(0, 0), Rotation);
            base.Move(axes.Y * Speed);
        }
    }
}

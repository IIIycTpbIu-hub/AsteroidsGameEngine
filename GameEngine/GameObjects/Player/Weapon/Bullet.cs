using GameEngine.Primitives;

namespace GameEngine.GameObjects.Player.Weapon
{
    public class Bullet : Ammunition
    {

        public Bullet(Point2D[] points, Point2D spawnPoint) : base(points, spawnPoint)
        {

        }

        protected internal override void Move()
        {
            Axes2D axes = new Axes2D(new Point2D(0, 0), Rotation);
            base.Move(axes.Y * base.Speed);
        }
    }
}

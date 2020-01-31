using GameEngine.Primitives;

namespace GameEngine.GameObjects.Enemies
{
    public class Enemy : GameObject
    {
        public Enemy(Point2D[] points2D, Point2D creationPoint2D) : base(points2D, creationPoint2D)
        {

        }

        protected internal virtual void Move()
        {

        }
    }
}

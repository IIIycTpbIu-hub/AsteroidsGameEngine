using GameEngine.Primitives;

namespace GameEngine.GameObjects.Player.Acceleration
{
    class Acceleration
    {
        public Point2D Vector2D { get; set; }

        public float Speed { get; set; }

        public int LifeTime { get; set; }

        public Acceleration(Point2D forceVector2D, float speed, int lifeTime)
        {
            Vector2D = forceVector2D;
            Speed = speed;
            LifeTime = lifeTime;
        }
    }
}

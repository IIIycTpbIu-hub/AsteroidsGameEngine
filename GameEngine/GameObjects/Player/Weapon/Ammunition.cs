using System;
using GameEngine.Primitives;

namespace GameEngine.GameObjects.Player.Weapon
{
    public class Ammunition : GameObject
    {
        private int _defaultLifeTime = 100;

        public int LifeTime
        {
            get { return _defaultLifeTime; }
            set
            {
                if (value >= 0)
                {
                    _defaultLifeTime = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Ammunition(Point2D[] points, Point2D spawnPoint) : base(points, spawnPoint)
        {

        }

        public void Update()
        {
            Move();
            CheckLifetime();
        }

        protected internal virtual void Move()
        {

        }

        private void CheckLifetime()
        { 
            if (LifeTime > 0)
            {

                LifeTime--;
            }
            else
            {
                base.OnDestroy();
            }
        }
    }
}

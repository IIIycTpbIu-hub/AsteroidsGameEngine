using System.Collections.Generic;
using GameEngine.Primitives;

namespace GameEngine.GameObjects.Player.Acceleration
{
    class AccelerationsController
    {
        private readonly GameObject _gameObject;
        private Point2D _accelerationVector2D;
        private readonly List<Acceleration> _accelerationsList;

        public AccelerationsController(GameObject gameObject)
        {
            _gameObject = gameObject;
            _accelerationsList = new List<Acceleration>();
        }

        public void Update()
        {
            List<Acceleration> removingAccelerations = new List<Acceleration>();
            if (_accelerationsList.Count != 0)
            {
                _accelerationVector2D = new Point2D(0, 0);
                foreach (var acceleration in _accelerationsList)
                {
                    _accelerationVector2D += acceleration.Vector2D * acceleration.Speed;
                    acceleration.LifeTime--;
                    if (acceleration.LifeTime == 0)
                    {
                        removingAccelerations.Add(acceleration);
                    }
                }


                _gameObject.Move(_accelerationVector2D);
            }

            foreach (var acceleration in removingAccelerations)
            {
                _accelerationsList.Remove(acceleration);
            }
        }

        public void AddAcceleration(Point2D vector2D, float speed, int lifeTime)
        {
            _accelerationsList.Add(new Acceleration(vector2D, speed, lifeTime));
        }
    }
}

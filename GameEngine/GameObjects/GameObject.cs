using System;
using GameEngine.Primitives;

namespace GameEngine.GameObjects
{
    public class GameObject
    {
        private float _speed = 1;

        public readonly GameEnginePolygon GameEnginePolygon;
        public readonly Collider Collider;

        public Point2D Pivot
        {
            get { return GameEnginePolygon.Pivot; }
        }

        public float Rotation
        {
            get { return GameEnginePolygon.AngleRotate; }
            set
            {
                GameEnginePolygon.RotatePolygon(value);
            }
        }

        public float Speed
        {
            get { return _speed; }
            set
            {
                if (value > 1)
                {
                    _speed = value;
                }
            }
        }

        public event EventHandler<GameObject> Destroy;

        public GameObject(Point2D[] points2D, Point2D creationPoint2D)
        {
            GameEnginePolygon = new GameEnginePolygon(points2D, creationPoint2D);
            Collider = new Collider(GameEnginePolygon);
        }

        public Point2D[] GetPointsForDrawing()
        {
            return GameEnginePolygon.Points;
        }

        public void Rotate(float angle)
        {
            Rotation = angle;
        }

        protected internal virtual void Move(Point2D vector)
        {
            GameEnginePolygon.SetDirectionVector(vector);
        }

        public void OnDestroy()
        {
            Destroy?.Invoke(this, this);
        }
    }
}

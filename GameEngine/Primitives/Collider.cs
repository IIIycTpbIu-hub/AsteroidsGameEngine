namespace GameEngine.Primitives
{
    public class Collider
    {
        private readonly GameEnginePolygon _enginePolygon;

        public Point2D Pivot
        {
            get { return _enginePolygon.Pivot; }
        }

        public float MaxRadius { get; private set; }

        public Point2D[] Points
        {
            get { return _enginePolygon.Points; }
        }

        public Collider(GameEnginePolygon enginePolygon)
        {
            _enginePolygon = enginePolygon;
            MaxRadius = FindMaxRadius(_enginePolygon.Points);
        }

        public bool IsCollision(Collider otherCollider)
        {
            if (IsRoughCalculatedCollision(otherCollider))
            {
                if (IsAccurateCalculatedCollision(otherCollider))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsRoughCalculatedCollision(Collider otherCollider)
        {
            float distance = Point2D.Distance(this.Pivot, otherCollider.Pivot);
            return distance < this.MaxRadius + otherCollider.MaxRadius ? true : false;
        }

        private bool IsAccurateCalculatedCollision(Collider collusionObject)
        {
            Point2D[] objectAPoints = Points;
            Point2D[] objectBPoints = collusionObject.Points;

            for (int i = 0; i < objectAPoints.Length - 1; i++)
            {
                Point2D a1 = objectAPoints[i];
                Point2D a2 = objectAPoints[i + 1];
                for (int j = 0; j < objectBPoints.Length - 1; j++)
                {
                    Point2D b1 = objectBPoints[j];
                    Point2D b2 = objectBPoints[j + 1];

                    if (AreLinesCross(a1, a2, b1, b2))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool AreLinesCross(Point2D a1, Point2D a2, Point2D b1, Point2D b2)
        {
            float v1 = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
            float v2 = (b2.X - b1.X) * (a2.Y - b1.Y) - (b2.Y - b1.Y) * (a2.X - b1.X);
            float v3 = (a2.X - a1.X) * (b1.Y - a1.Y) - (a2.Y - a1.Y) * (b1.X - a1.X);
            float v4 = (a2.X - a1.X) * (b2.Y - a1.Y) - (a2.Y - a1.Y) * (b2.X - a1.X);

            return (v1 * v2 < 0) && (v3 * v4 < 0) ? true : false;
        }

        private float FindMaxRadius(Point2D[] points)
        {
            float maxRadius = 0;
            float temp;
            for (int i = 0; i < points.Length; i++)
            {
                temp = Point2D.Distance(Pivot, points[i]);
                if (temp > maxRadius)
                {
                    maxRadius = temp;
                }
            }

            return maxRadius;
        }
    }
}

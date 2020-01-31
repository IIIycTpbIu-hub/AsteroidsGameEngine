using GameEngine.Primitives;

namespace GameEngine.GameObjects.Enemies
{
    public class UFO : Enemy
    {
        private readonly Player.Player _player;

        public UFO(Player.Player player, Point2D[] points2D, Point2D creationPoint2D) : base(points2D, creationPoint2D)
        {
            _player = player;
        }

        protected internal override void Move()
        {
            ChasePlayer();
        }

        private void ChasePlayer()
        {
            if (_player != null)
            {
                Point2D diractionVector = _player.Pivot - Pivot;
                float distance = Point2D.VectorDisnace(diractionVector);
                base.Move((diractionVector / distance) * Speed);
            }
        }
    }
}

using GameEngine.Primitives;

namespace GameEngine.GameObjects.Player.Weapon
{
    public class Laser : Ammunition
    {
        private readonly float _padding = 0.1f;
        private readonly Player _player;


        public Laser(Point2D[] points, Point2D spawnPoint, Player player, float padding) : base(points, spawnPoint)
        {
            _player = player;
            _padding = padding;
        }

        protected internal override void Move()
        {
            base.Rotate(_player.Rotation - Rotation);

            Axes2D axes = new Axes2D(new Point2D(0, 0), _player.Rotation);
            Point2D shotPoint = new Point2D(_player.Pivot.X, _player.Pivot.Y);
            shotPoint += axes.Y * _padding;

            base.Move(shotPoint - Pivot);
        }
    }
}

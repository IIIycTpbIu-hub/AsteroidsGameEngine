using System;
using GameEngine.Primitives;

namespace GameEngine.GameObjects.Player.Weapon
{
    public class Gun
    {
        private readonly Player _player;
        private readonly Point2D[] _laserPoints;
        private readonly int _laserLifeTime;
        private readonly Point2D[] _bulletPoints;
        private readonly int _bulletLifeTime;
        private readonly int _maxLaserBullets;
        private readonly int _laserRecoverTime;
        private int _time;

        public bool IsReadyToFire { get; set; }
        public int AvaibleLaserShots { get; private set; }

        public event EventHandler<int> LaserRecharged;

        public Gun(Player player, Point2D[] bulletPoints, int bulletLifeTime, Point2D[] laserPoints, int laserLifeTime, int maxLaserBullets, int laserRecoverTime)
        {
            _player = player;
            _laserLifeTime = laserLifeTime;
            _bulletLifeTime = bulletLifeTime;
            _laserPoints = laserPoints;
            _bulletPoints = bulletPoints;
            AvaibleLaserShots = maxLaserBullets;
            _maxLaserBullets = maxLaserBullets;
            _laserRecoverTime = laserRecoverTime;
            IsReadyToFire = true;
        }

        public Bullet FireWithBullet(float speed, float padding)
        {
            if (IsReadyToFire)
            {
                Axes2D axes = new Axes2D(new Point2D(0, 0), _player.Rotation);
                Point2D shotPoint = new Point2D(_player.Pivot.X, _player.Pivot.Y);
                shotPoint += axes.Y * padding;
                Bullet b = new Bullet(_bulletPoints, shotPoint);
                b.Speed = speed;
                b.Rotate(_player.Rotation);
                b.LifeTime = _bulletLifeTime;
                IsReadyToFire = false;
                return b;
            }

            return null;
        }

        public Laser FireWithLaser(float padding)
        {
            if (IsReadyToFire && AvaibleLaserShots > 0)
            {
                Axes2D axes = new Axes2D(new Point2D(0, 0), _player.Rotation);
                Point2D shotPoint = new Point2D(_player.Pivot.X, _player.Pivot.Y);
                shotPoint += axes.Y * padding;
                Laser laser = new Laser(_laserPoints, shotPoint, _player, padding);
                laser.LifeTime = _laserLifeTime;
                laser.Rotate(_player.Rotation);
                IsReadyToFire = false;
                AvaibleLaserShots--;

                return laser;
            }

            return null;
        }

        public void Update()
        {
            if (AvaibleLaserShots < _maxLaserBullets)
            {
                if (_time < _laserRecoverTime)
                {
                    _time++;
                }
                else
                {
                    _time = 0;
                    AvaibleLaserShots++;
                    LaserRecharged?.Invoke(this, AvaibleLaserShots);
                }
            }
        }
    }
}

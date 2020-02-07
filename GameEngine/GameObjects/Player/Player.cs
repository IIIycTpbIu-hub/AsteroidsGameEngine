using System;
using GameEngine.GameObjects.Player.Acceleration;
using GameEngine.GameObjects.Player.Weapon;
using GameEngine.Primitives;

namespace GameEngine.GameObjects.Player
{
    public class Player : GameObject
    {
        private readonly AccelerationsController _accelerationsController;
        public bool isUp = false;
        public bool isRoratingLeft = false;
        public bool isRotatingRight = false;
        
        public readonly Gun Gun;

        public event EventHandler<Ammunition> Fire;

        public Player(Point2D[] points, Point2D creationPoint, Point2D[] bulletPoints, int bulletLifeTime,
            Point2D[] laserPoints, int laserLifeTime, int maxLaserBullets,
            int laserRecoverTime) : base(points, creationPoint)
        {
            _accelerationsController = new AccelerationsController(this);
            Gun = new Gun(this, bulletPoints, bulletLifeTime, laserPoints, laserLifeTime,
                maxLaserBullets, laserRecoverTime);
        }

        public void Update()
        {
            _accelerationsController.Update();
            Gun.Update();
        }

        public void MoveForward(int lifetime)
        {
            Axes2D axes = new Axes2D(new Point2D(0, 0), Rotation);
            _accelerationsController.AddAcceleration(axes.Y, Speed, lifetime);
        }


        public new void Rotate(float angle)
        {
            base.Rotate(angle);
        }

        public void FireWithBullet(float speed, float padding)
        {
            Ammunition bullet = Gun.FireWithBullet(speed, padding);
            Fire?.Invoke(this, bullet);
        }

        public void FireWithLaser(float padding)
        {
            Ammunition bullet = Gun.FireWithLaser(padding);
            Fire?.Invoke(this, bullet);
        }

        public void SetOnReadyToFire()
        {
            Gun.IsReadyToFire = true;
        }
    }
}

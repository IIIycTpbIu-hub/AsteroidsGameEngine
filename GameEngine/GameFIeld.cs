using System;
using System.Collections.Generic;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Enemies;
using GameEngine.GameObjects.Player;
using GameEngine.GameObjects.Player.Weapon;
using GameEngine.Primitives;


namespace GameEngine.EnemyControll
{
    public class GameField
    {
        private readonly List<Enemy> _enemies;
        private readonly List<Ammunition> _bullets;
        private readonly List<GameObject> _removingObjects;
        private float _padding;
        private readonly EnemyController _enemyController;
        private readonly Point2D[] _asteroidPoints;
        private readonly Point2D[] _smallAsteroidPoints;
        private readonly Point2D[] _ufoPoints;
        private readonly float _enemiesSpeed;
        private readonly Random _rnd;

        public Player Player;

        public event EventHandler<GameObject> Collision;
        public event EventHandler<GameObject> CreateGameObject;
        public event EventHandler<GameObject> EnemyDestroyed;

        public GameField(float width, float height, float padding, Point2D[] asteroidPoints, Point2D[] smallAsteroidPoints,
            Point2D[] ufoPoints, float enemiesSpeed, int maxEnemiesInScene, int spawnDelayTime)
        {
            Width = width;
            Height = height;
            _asteroidPoints = asteroidPoints;
            _smallAsteroidPoints = smallAsteroidPoints;
            _ufoPoints = ufoPoints;
            _enemiesSpeed = enemiesSpeed;
            _padding = padding;
            _enemies = new List<Enemy>();
            _bullets = new List<Ammunition>();
            _removingObjects = new List<GameObject>();
            _enemyController = new EnemyController(this, maxEnemiesInScene, spawnDelayTime);
            _enemyController.CreateEnemy += OnCreateEnemy;
            _rnd = new Random();
        }

        public float Width { get; private set; }

        public float Height { get; private set; }

        public float Padding
        {
            get { return _padding; }
            set
            {
                if (value >= 0)
                {
                    _padding = value;
                }
            }
        }

        public Player CreatePlayer(Point2D[] points, Point2D creationPoint, Point2D[] bulletPoints, int bulletLifeTime, Point2D[] laserPoints, int laserLifeTime)
        {
            Player = new Player(points, creationPoint, bulletPoints, bulletLifeTime, laserPoints, laserLifeTime, 4, 500);
            Player.Destroy += OnDestroy;
            Player.Fire += AddBullet;
            CreateGameObject?.Invoke(this, Player);
            return Player;
        }

        public Enemy CreateAsteroid(Point2D[] points, Point2D creationPoint)
        {
            Enemy enemy = new Asteroid(points, creationPoint);
            enemy.Rotation = _rnd.Next(0, 360);
            enemy.Destroy += OnDestroy;
            enemy.Speed = _enemiesSpeed;
            _enemies.Add(enemy);
            CreateGameObject?.Invoke(this, enemy);
            return enemy;
        }

        public Enemy CreateUFO(Point2D[] points, Point2D creationPoint)
        {
            Enemy enemy = new UFO(Player, points, creationPoint);
            enemy.Destroy += OnDestroy;
            enemy.Rotate(180);
            enemy.Speed = _enemiesSpeed;
            _enemies.Add(enemy);
            CreateGameObject?.Invoke(this, enemy);
            return enemy;
        }

        public Enemy CreateSmallAsteroid(Point2D[] points, Point2D creationPoint)
        {
            Enemy enemy = new SmallAsteroid(points, creationPoint);
            enemy.Rotation = _rnd.Next(0, 360);
            enemy.Destroy += OnDestroy;
            enemy.Speed = _enemiesSpeed;
            _enemies.Add(enemy);
            CreateGameObject?.Invoke(this, enemy);
            return enemy;
        }

        public void OnCreateEnemy(object sender, EnemyController.EnemyType enemyType)
        {
            if (sender is Point2D)
            {
                CreateSmallAsteroid(_smallAsteroidPoints, (Point2D)sender);
            }

            if (enemyType == EnemyController.EnemyType.Asteroid)
            {
                CreateAsteroid(_asteroidPoints, CreateRandomOffScreenPoint());
            }

            if (enemyType == EnemyController.EnemyType.UFO)
            {
                CreateUFO(_ufoPoints, CreateRandomOffScreenPoint());
            }
        }

        public void AddBullet(object sender, GameObject gameObject)
        {
            if (gameObject is Ammunition)
            {
                gameObject.Destroy += OnDestroy;
                _bullets.Add(gameObject as Ammunition);
                CreateGameObject?.Invoke(this, gameObject);
            }
        }

        public void OnDestroy(object sender, GameObject gameObject)
        {
            Type t = gameObject.GetType();

            if (t == typeof(Player))
            {
                Player = null;
            }

            if (gameObject is Enemy)
            {
                EnemyDestroyed?.Invoke(this, gameObject);
            }

            _removingObjects.Add(gameObject);
        }

        public void Update()
        {
            CheckCollisions();
            CheckOffsetPosition();
            Player?.Update();
            MoveEnemies();
            MoveBullets();
            _enemyController.Update();
            CheckRemoving();
        }

        private void CheckObjectsPosition(GameObject gameObject)
        {
            float x = 0;
            float y = 0;

            if (gameObject.Pivot.X > Width + Padding)
                x = -gameObject.Pivot.X - Padding;
            if (gameObject.Pivot.X < -Padding)
                x = Width + Padding - gameObject.Pivot.X;
            if (gameObject.Pivot.Y > Height + Padding)
                y = -gameObject.Pivot.Y - Padding;
            if (gameObject.Pivot.Y < -Padding)
                y = Height - gameObject.Pivot.Y + Padding;
            gameObject.GameEnginePolygon.SetDirectionVector(new Point2D(x, y));
        }

        private void CheckOffsetPosition()
        {
            if (Player != null)
            {
                CheckObjectsPosition(Player);
            }

            if (_enemies?.Count > 0)
            {
                foreach (var enemy in _enemies)
                {
                    CheckObjectsPosition(enemy);
                }
            }

            if (_bullets?.Count > 0)
            {
                foreach (var bullet in _bullets)
                {
                    CheckObjectsPosition(bullet);
                }
            }
        }

        private void CheckCollisions()
        {
            if (_enemies != null && _enemies.Count > 0 && Player != null)
            {
                foreach (var enemy in _enemies)
                {
                    bool collision = enemy.Collider.IsCollision(Player.Collider);
                    if (collision)
                    {
                        Collision?.Invoke(enemy, Player);
                        break;
                    }
                }
            }

            if (_bullets?.Count > 0 && _enemies?.Count > 0)
            {
                foreach (var bullet in _bullets)
                {
                    foreach (var enemy in _enemies)
                    {
                        bool collision = bullet.Collider.IsCollision(enemy.Collider);
                        if (collision)
                        {
                            if (bullet.GetType() == typeof(Bullet))
                            {
                                bullet.OnDestroy();
                            }
                            enemy.OnDestroy();
                            return;
                        }
                    }
                }
            }
        }

        private void MoveEnemies()
        {
            if (_enemies?.Count > 0)
            {
                foreach (var enemy in _enemies)
                {
                    enemy.Move();
                }
            }
        }

        private void MoveBullets()
        {
            if (_bullets?.Count > 0)
            {
                foreach (var bullet in _bullets)
                {
                    bullet.Update();
                }
            }
        }

        private void CheckRemoving()
        {
            if (_removingObjects?.Count > 0)
            {
                foreach (var gameObject in _removingObjects)
                {
                    if (gameObject is Ammunition)
                    {
                        _bullets.Remove(gameObject as Ammunition);
                    }

                    if (gameObject is Enemy)
                    {
                        _enemies.Remove(gameObject as Enemy);
                    }
                }
                _removingObjects.Clear();
            }
        }

        private Point2D CreateRandomOffScreenPoint()
        {
            float x = _rnd.Next(0 - (int)Padding, (int)(Width + Padding));
            float y = _rnd.Next(0 - (int)Padding, (int)(Height + Padding));
            if (_rnd.Next(0, 1000) >= 500)
            {
                if (_rnd.Next(0, 1000) >= 500)
                {
                    x = 0 - Padding;
                }
                else
                {
                    x = Width + Padding;
                }
            }
            else
            {
                if (_rnd.Next(0, 1000) >= 500)
                {
                    y = 0 - Padding;
                }
                else
                {
                    y = Height + Padding;
                }
            }

            return new Point2D(x, y);
        }
    }
}

using System;
using System.Collections.Generic;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Enemies;

namespace GameEngine.EnemyControll
{
    public class EnemyController
    {
        private readonly int _maxEnemiesInScene;
        private readonly int _maxSpawnDelayTime;
        private readonly List<EnemyItem> _spawingEnemies = new List<EnemyItem>();
        private readonly List<EnemyItem> _removingEnemies = new List<EnemyItem>();
        private readonly Random _rnd;

        public enum EnemyType
        {
            Asteroid,
            UFO,
            SmallAsteroid
        };

        public int EnemiesCount { get; private set; }

        public event EventHandler<EnemyType> CreateEnemy;

        public EnemyController(GameField gameField, int maxEnemiesInScene, int maxSpawnDelayTime)
        {
            gameField.EnemyDestroyed += OnDestoryEnemy;
            gameField.CreateGameObject += OnCreateEnemy;
            _maxEnemiesInScene = maxEnemiesInScene;
            _maxSpawnDelayTime = maxSpawnDelayTime;
            EnemiesCount = 0;
            _rnd = new Random();

            for (int i = 0; i < _maxEnemiesInScene; i++)
            {
                _spawingEnemies.Add(new EnemyItem(EnemyType.Asteroid, _rnd.Next(0, _maxSpawnDelayTime)));
            }
        }

        public void Update()
        {
            foreach (var spawingEnemy in _spawingEnemies)
            {
                if (spawingEnemy.TimeToSpawn <= 0)
                {
                    CreateEnemy?.Invoke(this, spawingEnemy.EnemyType);
                    _removingEnemies.Add(spawingEnemy);
                }
                else
                {
                    spawingEnemy.TimeToSpawn--;
                }
            }
            CheckRemoving();
        }

        private void OnDestoryEnemy(object sender, GameObject gameObject)
        {
            Type t = gameObject.GetType();
            if (t != typeof(SmallAsteroid))
            {
                EnemiesCount--;

                if (t == typeof(Asteroid))
                {
                    int smallAsteroids = _rnd.Next(2, 4);
                    for (int i = 0; i < smallAsteroids; i++)
                    {
                        CreateEnemy?.Invoke(gameObject.Pivot, EnemyType.SmallAsteroid);
                    }

                }

                EnemyType enemyType = ChoseEnemyType();

                if (EnemiesCount < _maxEnemiesInScene)
                {
                    AddEnemyToSpawnQueue(enemyType, _rnd.Next(0, _maxSpawnDelayTime));
                }
            }
        }

        private void OnCreateEnemy(object sender, GameObject gameObject)
        {
            Type t = gameObject.GetType();
            if (gameObject is Enemy && t != typeof(SmallAsteroid))
            {
                EnemiesCount++;
            }
        }

        private EnemyType ChoseEnemyType()
        {
            Random rnd = new Random();
            EnemyType type = (EnemyType)rnd.Next(0, Enum.GetNames(typeof(EnemyType)).Length - 1);
            return type;
        }

        private void AddEnemyToSpawnQueue(EnemyType enemyType, int timeToSpawn)
        {
            EnemyItem enemy = new EnemyItem(enemyType, timeToSpawn);
            _spawingEnemies.Add(enemy);
        }

        private void CheckRemoving()
        {
            if (_removingEnemies.Count > 0)
            {
                foreach (var removing in _removingEnemies)
                {
                    _spawingEnemies.Remove(removing);
                }
                _removingEnemies.Clear();
            }
        }
    }
}

namespace GameEngine.EnemyControll
{
    public class EnemyItem
    {
        public EnemyController.EnemyType EnemyType { get; set; }
        public int TimeToSpawn { get; set; }

        public EnemyItem(EnemyController.EnemyType enemyType, int timeToSpawn)
        {
            EnemyType = enemyType;
            TimeToSpawn = timeToSpawn;
        }
    }
}

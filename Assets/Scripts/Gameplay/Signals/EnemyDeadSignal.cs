namespace Gameplay.Signals
{
    public class EnemyDeadSignal
    {
        public Enemy Enemy { get; }

        public EnemyDeadSignal(Enemy enemy)
        {
            Enemy = enemy;
        }
    }
}
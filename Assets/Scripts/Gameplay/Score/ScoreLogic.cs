using System;
using Core.Configs;
using Gameplay.Players;
using Gameplay.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Score
{
    public class ScoreLogic : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private Player _player;
        private float _maxY;
        private ScoreConfig _config;
        
        public int Score { get; private set; }

        public ScoreLogic(SignalBus signalBus, Player player, ScoreConfig config)
        {
            _signalBus = signalBus;
            _player = player;
            _config = config;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyDeadSignal>(OnEnemyDeath);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDeadSignal>(OnEnemyDeath);
        }

        public void CalculateHeightScore()
        {
            float currentY = _player.transform.position.y;

            if (currentY > _maxY)
            {
                float diff = currentY - _maxY;
                int newScore = Convert.ToInt32(diff * _config.ScoreCoef);
                AddScore(newScore);

                _maxY = currentY;
            }
        }
        
        private void AddScore(int score)
        {
            Score += score;
        }

        private void OnEnemyDeath(EnemyDeadSignal signal)
        {
            AddScore(signal.Enemy.Score);
        }
    }
}
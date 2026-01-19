using System;
using Gameplay.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private float _scoreCoef;

        private int _score;
        private SignalBus _signalBus;
        private Player _player;
        private float _maxY;

        [Inject]
        public void Construct(SignalBus signalBus, Player player)
        {
            _signalBus = signalBus;
            _player = player;
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<EnemyDeadSignal>(OnEnemyDeath);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<EnemyDeadSignal>(OnEnemyDeath);
        }

        private void Update()
        {
            CalculateHeightScore();
            _scoreText.text = _score.ToString();
        }

        private void CalculateHeightScore()
        {
            float currentY = _player.transform.position.y;

            if (currentY > _maxY)
            {
                float diff = currentY - _maxY;
                int newScore = Convert.ToInt32(diff * _scoreCoef);
                AddScore(newScore);

                _maxY = currentY;
            }
        }

        private void AddScore(int score)
        {
            _score += score;
        }

        private void OnEnemyDeath(EnemyDeadSignal signal)
        {
            AddScore(signal.Enemy.Score);
        }
    }
}
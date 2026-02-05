using Gameplay.Score;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private ScoreLogic _scoreLogic;

        [Inject]
        private void Construct(ScoreLogic scoreLogic)
        {
            _scoreLogic = scoreLogic;
        }

        private void Update()
        {
            _scoreLogic.CalculateHeightScore();
            _scoreText.text = _scoreLogic.Score.ToString();
        }
    }
}
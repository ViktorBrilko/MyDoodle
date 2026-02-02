using Gameplay.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class RestartPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _restartPanel;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDeath);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDeath);
        }

        private void OnPlayerDeath()
        {
            _restartPanel.SetActive(true);
        }

        public void RestartLevel()
        {
            _restartPanel.SetActive(false);
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevel);
        }
    }
}
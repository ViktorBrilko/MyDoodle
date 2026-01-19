using Gameplay.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Color _pausedBackgroundColor;
    [SerializeField] private Button _pausedButton;

    private bool _isPaused;
    private float _regularTime;
    private Color _regularBackgroundColor;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    
    private void Awake()
    {
        _regularTime = Time.timeScale;
        _regularBackgroundColor = _background.color;
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
        _pausedButton.interactable = false;
    }

    public void PauseGame()
    {
        if (_isPaused)
        {
            Time.timeScale = _regularTime;
            _isPaused = false;
            _background.color = _regularBackgroundColor;
        }
        else
        {
            Time.timeScale = 0;
            _isPaused = true;
            _background.color = _pausedBackgroundColor;
        }
    }
}
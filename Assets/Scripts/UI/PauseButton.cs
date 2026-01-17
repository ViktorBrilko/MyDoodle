using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Color _pausedBackgroundColor;

    private bool _isPaused;
    private float _regularTime;
    private Color _regularBackgroundColor;

    private void Awake()
    {
        _regularTime = Time.timeScale;
        _regularBackgroundColor = _background.color;
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
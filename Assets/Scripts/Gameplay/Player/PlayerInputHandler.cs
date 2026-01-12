using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Update()
    {
        float direction = Input.GetAxis("Horizontal");
        _player.Move(direction);

        if (Input.GetKeyDown(KeyCode.E))
        {
            _player.Fire();
        }
        
        _player.Jump();
    }
}
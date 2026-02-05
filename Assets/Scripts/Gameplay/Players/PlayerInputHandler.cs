using UnityEngine;

namespace Gameplay.Players
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private PlayerMovement _playerMovement;

        private void Update()
        {
            float direction = Input.GetAxis("Horizontal");
            _playerMovement.Move(direction);

            if (Input.GetKeyDown(KeyCode.E))
            {
                _player.Fire();
            }
        
            _playerMovement.Jump();
            
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) {
                _player.BecomeInvincible(1000);
            }
#endif
        }
    }
}
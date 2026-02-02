using UnityEngine;

namespace Gameplay.Players
{
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
            
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) {
                _player.BecomeInvincible(1000);
            }
#endif
        }
    }
}
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class CameraFollow : MonoBehaviour
    {
        private Player _player;
        private CinemachineVirtualCamera _camera;
        private float _cameraBottom;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
            _camera = GetComponent<CinemachineVirtualCamera>();

            _camera.Follow = _player.transform;
            _cameraBottom = Camera.main.ViewportToWorldPoint(Vector2.zero).y;
        }

        private void Update()
        {
            CheckPlayerYPosition();
        }

        private void CheckPlayerYPosition()
        {
            if (_player.Rigidbody2D.velocity.y > 0)
            {
                _cameraBottom = Camera.main.ViewportToWorldPoint(Vector2.zero).y;
            }

            if (_player.transform.position.y < _cameraBottom && _player.IsAlive)
            {
                Debug.Log("игрок умер от падения");
                _player.Die();
            } 
        }
    }
}
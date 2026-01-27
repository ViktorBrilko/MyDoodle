using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Boosts
{
    public class Jetpack : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _flightTime;

        Player _player;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                // StartCoroutine(FlyRoutine(player));

                player.BecomeInvincible(_flightTime);

                _player = player;
            }
        }

             if (_player != null)          
            {
                _player.transform.Translate(Vector3.up * _speed * Time.deltaTime);
                StartCoroutine(WaitTime());
            }
                
        }

        private IEnumerator WaitTime()
        {
            _player.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            yield return new WaitForSeconds(_flightTime);
            _player.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _player = null;
            
        }

        private IEnumerator FlyRoutine(Player player)
        {
            player.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            Vector3 end = new Vector3(player.transform.position.x, player.transform.position.y + 20,
                player.transform.position.z);

            float elapsed = 0f;
            while (elapsed < _flightTime)
            {
                if (Vector3.Distance(player.transform.position, end) < 1f)
                {
                    player.transform.position = end;
                    break;
                }

                player.transform.position = Vector3.Lerp(player.transform.position, end, elapsed / _flightTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            player.transform.position = end;

            player.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            Destroy(gameObject);
        }
    }
}
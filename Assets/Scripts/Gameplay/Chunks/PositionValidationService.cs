using Core.Configs;
using UnityEngine;

namespace Gameplay.Chunks
{
    public class PositionValidationService
    {
        private float _rightSideOfScreenInWorld;
        private float _leftSideOfScreenInWorld;
        private float _itemXDistanceCoef;
        private ChunkConfig _config;

        public PositionValidationService(ChunkConfig config)
        {
            _rightSideOfScreenInWorld =
                Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
            _leftSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
            _config = config;
        }

        public Vector3 GetValidPosition(float objectWidth, float currentY, ChunkPresentation chunk)
        {
            bool isValidPosition = false;
            int attempts = 0;
            _itemXDistanceCoef = objectWidth * 2 + _config.ItemXDistanceCoef;

            Vector3 position = new Vector3();

            while (!isValidPosition && attempts <= _config.MaxPositionAttempts)
            {
                position = new Vector3(
                    Random.Range(_rightSideOfScreenInWorld - objectWidth,
                        _leftSideOfScreenInWorld + objectWidth),
                    currentY, chunk.transform.position.z);

                if (chunk.Logic.ItemsPositions.Count == 0) break;

                foreach (Vector2 existingPosition in chunk.Logic.ItemsPositions)
                {
                    if (Mathf.Approximately(position.y, existingPosition.y) &&
                        Mathf.Abs(position.x - existingPosition.x) < _itemXDistanceCoef)
                    {
                        attempts++;
                        isValidPosition = false;
                        break;
                    }

                    isValidPosition = true;
                }

                if (attempts >= _config.MaxPositionAttempts)
                {
                    Debug.Log("Количество попыток разместить объект исчерпано");
                }
            }

            return position;
        }
    }
}
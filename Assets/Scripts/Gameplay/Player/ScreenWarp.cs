using UnityEngine;

public class ScreenWarp : MonoBehaviour
{
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        float rightSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        float leftSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;

        if (screenPos.x <= 0)
        {
            transform.position = new Vector3(rightSideOfScreenInWorld, transform.position.y, transform.position.z);
        }
        else if (screenPos.x >= Screen.width)
        {
            transform.position = new Vector3(leftSideOfScreenInWorld, transform.position.y, transform.position.z);
        }
    }
}
using UnityEngine;

public class ScreenWarp : MonoBehaviour
{
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        float rightSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        float leftSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
        
        float bottomSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        float topSideOfScreenInWorld = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).y;

        if (screenPos.x <= 0)
        {
            transform.position = new Vector2(rightSideOfScreenInWorld, transform.position.y);
        }
        else if (screenPos.x >= Screen.width)
        {
            transform.position = new Vector2(leftSideOfScreenInWorld, transform.position.y);
        }
    }
}
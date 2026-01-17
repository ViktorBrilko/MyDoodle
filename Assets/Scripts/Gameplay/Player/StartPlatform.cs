using UnityEngine;

public class StartPlatform : MonoBehaviour
{
    [SerializeField] private float _yCameraOffset;

    private void Start()
    {
        transform.SetParent(null);
    }

    private void Update()
    {
        if (CheckChunkVisibility())
        {       
           Destroy(gameObject);
        }
    }
    
    private bool CheckChunkVisibility()
    {
        return Camera.main.WorldToViewportPoint(transform.position).y <= _yCameraOffset;
    }
}
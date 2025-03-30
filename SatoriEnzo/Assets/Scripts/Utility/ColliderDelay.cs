using UnityEngine;

public class ColliderDelay : MonoBehaviour
{
    public float delay = 0.5f;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            Invoke(nameof(EnableCollider), delay);
        }
    }

    private void EnableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }
}

using UnityEngine;

public class CameraZoomOnDeath : MonoBehaviour
{
    private Transform target;
    private bool isZooming = false;

    public float zoomSpeed = 2f;
    public float targetSize = 2f;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void FocusOn(Transform t)
    {
        target = t;
        isZooming = true;
    }

    void Update()
    {
        if (!isZooming || target == null) return;

        // Position smoothly toward player
        Vector3 newPos = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), Time.unscaledDeltaTime * zoomSpeed);
        transform.position = newPos;

        // Zoom in
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.unscaledDeltaTime * zoomSpeed);
    }
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaypointPath
{
    public List<Transform> waypoints = new();
}

public class BossMovement : MonoBehaviour
{
    [Header("Path Patrol")]
    public List<WaypointPath> paths;
    public float moveSpeed = 3f;
    private int currentPathIndex = -1;
    private int currentPointIndex = 0;

    [Header("Breathing Float")]
    public float floatAmplitude = 0.3f;
    public float floatSpeed = 2f;
    private float floatOffset;

    [Header("Perspective Scale")]
    public float minY = -2f;
    public float maxY = 5f;
    public float minScale = 0.6f;
    public float maxScale = 1.3f;

    private Vector3 baseScale;
    private int lastPathIndex = -1;

    void Start()
    {
        baseScale = transform.localScale;
        floatOffset = Random.Range(0f, Mathf.PI * 2f);
        PickRandomPath();
    }

    void Update()
    {
        MoveAlongPath();
        ApplyBreathingFloat();
        ApplyPerspectiveScale();
    }

    void PickRandomPath()
    {
        if (paths.Count == 0) return;

        int newIndex = lastPathIndex;
        while (paths.Count > 1 && newIndex == lastPathIndex)
        {
            newIndex = Random.Range(0, paths.Count);
        }

        currentPathIndex = newIndex;
        currentPointIndex = 0;
        lastPathIndex = newIndex;
    }

    void MoveAlongPath()
    {
        if (currentPathIndex == -1 || paths[currentPathIndex].waypoints.Count == 0) return;

        Transform target = paths[currentPathIndex].waypoints[currentPointIndex];
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            currentPointIndex++;
            if (currentPointIndex >= paths[currentPathIndex].waypoints.Count)
            {
                PickRandomPath();
            }
        }
    }

    void ApplyBreathingFloat()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed + floatOffset) * floatAmplitude;
        transform.position += new Vector3(0, yOffset * Time.deltaTime, 0);
    }

    void ApplyPerspectiveScale()
    {
        float t = Mathf.InverseLerp(maxY, minY, transform.position.y); // lower = bigger
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = baseScale * scale;
    }
    private void OnDrawGizmos()
    {
        if (paths == null) return;
        int p = 0;
        foreach (var path in paths)
        {
            Color[] colors = { Color.cyan, Color.green, Color.magenta, Color.yellow };
            Gizmos.color = colors[p % colors.Length];
            p++;
            if (path == null || path.waypoints == null || path.waypoints.Count < 2) continue;

            for (int i = 0; i < path.waypoints.Count - 1; i++)
            {
                Transform a = path.waypoints[i];
                Transform b = path.waypoints[i + 1];

                if (a != null && b != null)
                {
                    Gizmos.DrawLine(a.position, b.position);
                }
            }

            foreach (var wp in path.waypoints)
            {
                if (wp != null)
                {
                    Gizmos.DrawSphere(wp.position, 0.1f);
                }
            }
        }
    }

}

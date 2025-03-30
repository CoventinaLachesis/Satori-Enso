using UnityEngine;
using System.Collections.Generic;

public class GatlingGunPartSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] gatlingParts; // 3 part prefabs
    [SerializeField] private string[] groundTags = { "Ground", "Platform" };
    [SerializeField] private float surfaceOffset = 0.5f;
    [SerializeField] private int maxAttempts = 100; // fail-safe in case spawn area is too small
    [SerializeField] private GameObject gatlingBullet;
    public float spawnBulletInterval;
    private List<Collider2D> groundColliders = new();
    private List<GameObject> spawnedParts = new(); // Stores spawned parts for despawning
    private List<GameObject> spawnedBullet = new();


    private void Awake()
    {
        FindGrounds();
    }

    public void StartPuzzle() { 
        SpawnParts();
    }
    public void EndPuzzle() {
        DespawnAll();
    }

    private void FindGrounds()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            foreach (string tag in groundTags)
            {
                if (obj.CompareTag(tag) && obj.TryGetComponent(out Collider2D col))
                {
                    groundColliders.Add(col);
                }
            }
        }
    }

    public void SpawnParts()
    {
        if (gatlingParts.Length == 0 || groundColliders.Count == 0)
        {
            Debug.LogWarning("No parts or ground colliders found.");
            return;
        }

        HashSet<Vector2> usedPositions = new();

        for (int i = 0; i < gatlingParts.Length; i++)
        {
            int attempts = 0;
            Vector2 spawnPos = Vector2.zero;
            bool valid = false;

            while (attempts < maxAttempts && !valid)
            {
                // Pick a random ground
                Collider2D col = groundColliders[Random.Range(0, groundColliders.Count)];
                Bounds bounds = col.bounds;

                float x = Random.Range(bounds.min.x, bounds.max.x);
                float y = bounds.max.y + surfaceOffset;
                spawnPos = new Vector2(x, y);

                if (!usedPositions.Contains(spawnPos))
                {
                    usedPositions.Add(spawnPos);
                    valid = true;
                }

                attempts++;
            }

            GameObject part = Instantiate(gatlingParts[i], spawnPos, Quaternion.identity);
            spawnedParts.Add(part); // Store for despawning        }
        }
    }
    public void SpawnBullet() {
        Vector2 spawnPos = Vector2.zero;

        // Pick a random ground
        Collider2D col = groundColliders[Random.Range(0, groundColliders.Count)];
        Bounds bounds = col.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y + surfaceOffset;
        spawnPos = new Vector2(x, y);

        

        GameObject bullet = Instantiate(gatlingBullet, spawnPos, Quaternion.identity);
        spawnedBullet.Add(bullet);

    }

    private void DespawnAll() {
        foreach (GameObject part in spawnedParts)
        {
            if (part != null) Destroy(part);
        }
        foreach (GameObject bullet in spawnedBullet)
        {
            if (bullet != null) Destroy(bullet);
        }
        spawnedParts.Clear();
        spawnedBullet.Clear();

    }
}

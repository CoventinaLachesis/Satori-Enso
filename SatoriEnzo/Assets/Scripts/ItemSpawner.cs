using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] positiveItemPrefabs;
    public GameObject[] negativeItemPrefabs;
    public Vector2 spawnMin; // Bottom Left
    public Vector2 spawnMax; // Top Right
    public float spawnInterval;
    private ItemType nextItem = ItemType.Positive;

    void Start()
    {
        InvokeRepeating("SpawnItem", spawnInterval, spawnInterval);
    }

    void SpawnItem()
    {
        float randomX = Random.Range(spawnMin.x, spawnMax.x);
        float randomY = Random.Range(spawnMin.y, spawnMax.y);
        Vector3 spawnPoint = new Vector3(randomX, randomY, 0);

        if(nextItem == ItemType.Positive)
        {
            nextItem = ItemType.Negative;
            Instantiate(positiveItemPrefabs[Random.Range(0, positiveItemPrefabs.Length)], spawnPoint, Quaternion.identity);
        }
        else 
        {
            nextItem = ItemType.Positive;
            Instantiate(negativeItemPrefabs[Random.Range(0, negativeItemPrefabs.Length)], spawnPoint, Quaternion.identity);
        }
    }

}

public enum ItemType
{
    Positive,
    Negative
}

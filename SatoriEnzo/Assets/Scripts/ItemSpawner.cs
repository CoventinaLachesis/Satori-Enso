using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] positiveItemPrefabs;
    public GameObject[] negativeItemPrefabs;
    public SpawnArea[] spawnAreas;
    public float spawnInterval;
    private ItemType nextItem = ItemType.Positive;

    void Start()
    {
        InvokeRepeating("SpawnItem", spawnInterval, spawnInterval);
    }

    void SpawnItem()
    {
        SpawnArea area = spawnAreas[Random.Range(0, spawnAreas.Length)];

        float randomX = Random.Range(area.min.x, area.max.x);
        float randomY = Random.Range(area.min.y, area.max.y);
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

[System.Serializable]
public struct SpawnArea
{
    public Vector2 min;
    public Vector2 max;
}

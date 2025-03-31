using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementPosition : PlatformPattern
{
    [SerializeField] private GameObject[] platformPrefabs;
    
    [SerializeField] private Vector2[] spawnPositions;
    [SerializeField] private Vector2[] constantPositions;
    public float speed;
    private PlatformMovementState state = PlatformMovementState.None;
    private GameObject[] platforms;
    public override void StartPattern()
    {
        if(platformPrefabs == null) return;
        
        platforms = new GameObject[platformPrefabs.Length];

        for(int i = 0; i < platformPrefabs.Length; i++)
        {
            platforms[i] = Instantiate(platformPrefabs[i], new Vector3(spawnPositions[i].x, spawnPositions[i].y, 0), Quaternion.identity);
        }
        state = PlatformMovementState.Start;
    }

    public override void EndPattern()
    {
        state = PlatformMovementState.End;
    }

    void Update()
    {
        if(state == PlatformMovementState.Start) MoveAtStart();
        if(state == PlatformMovementState.End) MoveAtEnd();
    }

    private void MoveAtStart()
    {
        for(int i = 0; i < platforms.Length; i++)
        {
            Vector3 movingVector = Vector3.MoveTowards(platforms[i].gameObject.transform.position, constantPositions[i], speed * Time.deltaTime);
            platforms[i].gameObject.transform.position = movingVector;
        }
    }

    private void MoveAtEnd()
    {
        for(int i = 0; i < platforms.Length; i++)
        {
            Vector3 movingVector = Vector3.MoveTowards(platforms[i].gameObject.transform.position, spawnPositions[i], speed * Time.deltaTime);
            platforms[i].gameObject.transform.position = movingVector;
        }
    }
}

public enum PlatformMovementState
{
    Start,
    End,
    None // When Platforms are not initiated
}
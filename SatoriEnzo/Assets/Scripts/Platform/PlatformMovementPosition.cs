using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementPosition : PlatformPattern
{
    [SerializeField] private MovingPlatform[] movingPlatforms;
    
    private PlatformMovementState state = PlatformMovementState.None;
    private GameObject[] platforms;
    public override void StartPattern()
    {
        if(movingPlatforms == null) return;
        
        platforms = new GameObject[movingPlatforms.Length];

        for(int i = 0; i < movingPlatforms.Length; i++)
        {
            platforms[i] = Instantiate(
                movingPlatforms[i].platformPrefab, 
                new Vector3(movingPlatforms[i].spawnPosition.x, movingPlatforms[i].spawnPosition.y, 0), 
                Quaternion.identity
            );
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
            Vector3 movingVector = Vector3.MoveTowards(
                platforms[i].gameObject.transform.position, 
                movingPlatforms[i].stagePosition, 
                movingPlatforms[i].enterSpeed * Time.deltaTime
            );
            platforms[i].gameObject.transform.position = movingVector;
        }
    }

    private void MoveAtEnd()
    {
        for(int i = 0; i < platforms.Length; i++)
        {
            Vector3 movingVector = Vector3.MoveTowards(
                platforms[i].gameObject.transform.position, 
                movingPlatforms[i].spawnPosition, 
                movingPlatforms[i].enterSpeed * Time.deltaTime
            );
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

[System.Serializable]
public struct MovingPlatform
{
    public GameObject platformPrefab;
    public Vector2 spawnPosition;
    public Vector2 stagePosition;
    public float enterSpeed;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementLoop : PlatformPattern
{
    [SerializeField] private LoopPlatform[] loopPlatforms;
    private GameObject[] platforms;
    public override void StartPattern()
    {
        if(loopPlatforms == null) return;
        
        platforms = new GameObject[loopPlatforms.Length];

        for(int i = 0; i < loopPlatforms.Length; i++)
        {
            platforms[i] = Instantiate(
                loopPlatforms[i].platformPrefab, 
                new Vector3(loopPlatforms[i].spawnPosition.x, loopPlatforms[i].spawnPosition.y, 0), 
                Quaternion.identity
            );

            loopPlatforms[i].State = PlatformMovementState.Start;
        }
    }

    public override void EndPattern()
    {
        for(int i = 0; i < platforms.Length; i++)
        {
            loopPlatforms[i].State = PlatformMovementState.End;
        }
    }

    void Update()
    {
        if(platforms == null) return;

        for(int i = 0; i < platforms.Length; i++)
        {
            MovePlatform(i);
        }
    }

    private void MovePlatform(int i)
    {
        if(loopPlatforms[i].State == PlatformMovementState.None) return;

        if(loopPlatforms[i].State == PlatformMovementState.Start)
        {
            if( Vector3.Distance(loopPlatforms[i].loopPositions[0], platforms[i].gameObject.transform.position) < 0.1f ) 
            {
                loopPlatforms[i].State = PlatformMovementState.Loop;
            }
                
            MoveToward(platforms[i], loopPlatforms[i].loopPositions[0], loopPlatforms[i].enterSpeed);
        }
            

        if(loopPlatforms[i].State == PlatformMovementState.Loop)
        {
            if( Vector3.Distance(loopPlatforms[i].loopPositions[loopPlatforms[i].NextPositionIndex], platforms[i].gameObject.transform.position) < 0.1f ) 
            {
                loopPlatforms[i].NextPositionIndex = (loopPlatforms[i].NextPositionIndex + 1) % loopPlatforms[i].loopPositions.Length;
            }

            MoveToward(platforms[i], loopPlatforms[i].loopPositions[loopPlatforms[i].NextPositionIndex], loopPlatforms[i].loopSpeed);
        }

        if(loopPlatforms[i].State == PlatformMovementState.End)
        {
            if( Vector3.Distance(loopPlatforms[i].spawnPosition, platforms[i].gameObject.transform.position) < 0.1f ) 
            {
                Destroy(platforms[i]);
                loopPlatforms[i].State = PlatformMovementState.None;
                return;
            }

            MoveToward(platforms[i], loopPlatforms[i].spawnPosition, loopPlatforms[i].enterSpeed);
        }
        
    }

    private void MoveToward(GameObject platform, Vector2 target, float speed)
    {
        Vector3 movingVector = Vector3.MoveTowards(
            platform.gameObject.transform.position,
            target, 
            speed * Time.deltaTime
        );
        platform.gameObject.transform.position = movingVector;
    }
}

[System.Serializable]
public class LoopPlatform
{
    public GameObject platformPrefab;
    public Vector2 spawnPosition;
    public Vector2[] loopPositions;
    public float enterSpeed;
    public float loopSpeed;
    private PlatformMovementState state = PlatformMovementState.None;
    private int nextPositionIndex = 0;

    public int NextPositionIndex
    {
        get{ return nextPositionIndex; }
        set{ nextPositionIndex = value; }
    }

    public PlatformMovementState State
    {
        get{ return state; }
        set{ state = value; }
    }
}

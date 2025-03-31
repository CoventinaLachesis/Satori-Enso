using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementPosition : PlatformPattern
{
    [SerializeField] private MovingPlatform[] movingPlatforms;
    
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
            movingPlatforms[i].State = PlatformMovementState.Start;
        }
    }

    public override void EndPattern()
    {
        for(int i = 0; i < movingPlatforms.Length; i++)
        {
            movingPlatforms[i].State = PlatformMovementState.End;
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
        if(movingPlatforms[i].State == PlatformMovementState.None || movingPlatforms[i].State == PlatformMovementState.Stop) return;

        if(movingPlatforms[i].State == PlatformMovementState.Start)
        {
            if( Vector3.Distance(movingPlatforms[i].stagePosition, platforms[i].gameObject.transform.position) < 0.1f )
            {
                platforms[i].gameObject.transform.position = movingPlatforms[i].stagePosition;
                movingPlatforms[i].State = PlatformMovementState.Stop;
                return;
            }

            MoveToward(platforms[i], movingPlatforms[i].stagePosition, movingPlatforms[i].enterSpeed);
        }

        if(movingPlatforms[i].State == PlatformMovementState.End)
        {
            if( Vector3.Distance(movingPlatforms[i].spawnPosition, platforms[i].gameObject.transform.position) < 0.1f )
            {
                Destroy(platforms[i]);
                movingPlatforms[i].State = PlatformMovementState.None;
                return;
            }

            MoveToward(platforms[i], movingPlatforms[i].spawnPosition, movingPlatforms[i].enterSpeed);
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

[System.Serializable]
public class MovingPlatform
{
    public GameObject platformPrefab;
    public Vector2 spawnPosition;
    public Vector2 stagePosition;
    public float enterSpeed;
    private PlatformMovementState state = PlatformMovementState.None;

    public PlatformMovementState State
    {
        get{ return state; }
        set{ state = value; }
    }
}
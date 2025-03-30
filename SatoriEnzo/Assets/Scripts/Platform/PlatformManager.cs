using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject[] platformMovingPatterns;
    public float spawnInterval;

    void Start()
    {
        StartCoroutine(SpawnPlatforms(spawnInterval));
    }

    IEnumerator SpawnPlatforms(float duration)
    {
        yield return new WaitForSeconds(duration);

        GameObject pattern = platformMovingPatterns[Random.Range(0, platformMovingPatterns.Length)];
        PlatformPattern patternScript = pattern.GetComponent<PlatformPattern>();
        patternScript.StartPattern();
        StartCoroutine(EndPattern(patternScript.duration, patternScript));
    }

    IEnumerator EndPattern(float duration, PlatformPattern patternScript)
    {
        yield return new WaitForSeconds(duration);
        patternScript.EndPattern(); 
        StartCoroutine(SpawnPlatforms(spawnInterval));
    }

}

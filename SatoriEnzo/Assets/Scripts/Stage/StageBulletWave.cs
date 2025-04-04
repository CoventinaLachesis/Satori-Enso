using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBulletWave : MonoBehaviour
{
    public GameObject[] bullets;
    public float resetInterval = 8;
    private bool isActive = false;

    public void StartShooting() 
    {
        isActive = true;
        StartCoroutine(SpawnWave());
        StartCoroutine(ResetWave());
    }

    IEnumerator SpawnWave()
    {
        List<StageBullet> sortedBullets = new List<StageBullet>();

        if(!isActive)
        {
            yield break;
        }

        foreach (var bullet in bullets)
        {
            StageBullet bulletScript = bullet.GetComponent<StageBullet>();

            if(bulletScript == null)
            {
                Debug.LogError("Cannot find stage bullet script");
                yield break;
            }

            sortedBullets.Add(bulletScript);
        }

        sortedBullets.Sort((a, b) => a.delay.CompareTo(b.delay));

        float currentTime = 0f;
        int index = 0;

        while (index < sortedBullets.Count)
        {
            float nextSpawnTime = sortedBullets[index].delay;

            yield return new WaitForSeconds(nextSpawnTime - currentTime);
            currentTime = nextSpawnTime;

            while (index < sortedBullets.Count && sortedBullets[index].delay == currentTime)
            {
                sortedBullets[index].Activate();
                index++;
            }
        }
    }

    IEnumerator ResetWave()
    {
        yield return new WaitForSeconds(resetInterval);
        isActive = false;

        foreach(var bullet in bullets)
        {
            StageBullet bulletScript = bullet.GetComponent<StageBullet>();

            if(bulletScript == null)
            {
                Debug.LogError("Cannot find stage bullet script");
                yield break;
            }

            bulletScript.Deactivate();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBulletSpawner : MonoBehaviour
{
    public GameObject[] bulletWaves;
    public float waveInterval = 10;

    void Start()
    {
        InvokeRepeating("StartShooting", 0, waveInterval);
    }

    void StartShooting()
    {
        StageBulletWave bulletWaveScript = bulletWaves[Random.Range(0, bulletWaves.Length)].GetComponent<StageBulletWave>();
        if(bulletWaveScript == null)
        {
            Debug.LogError("Cannot find stage bullet wave script");
        }
        bulletWaveScript.StartShooting();
    }
}

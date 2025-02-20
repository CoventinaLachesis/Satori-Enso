using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBulletWave : MonoBehaviour
{
    public GameObject[] bullets;

    void Start()
    {
        
    }

    void StartShooting() 
    {
        foreach (GameObject bullet in bullets)
        {
            StageBullet bulletScript = bullet.GetComponent<StageBullet>();

            if(bulletScript == null)
            {
                Debug.LogError("Cannot find stage bullet script");
                break;
            }

            bulletScript.Activate();
        }
    }
}

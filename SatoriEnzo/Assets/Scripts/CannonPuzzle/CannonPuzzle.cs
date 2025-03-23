using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPuzzle : Puzzle
{
    public GameObject cannonPrefab;
    public GameObject cannonBallItemPrefab;
    public GameObject cannonBallProjectilePrefab;

    private GameObject cannon;
    private GameObject cannonBallItem;
    private GameObject cannonBallProjectile;

    private Cannon cannonScript;
    private CannonBallItem cannonBallItemScript;
    private CannonBallProjectile cannonBallProjectileScript;
    private bool isActive;
    private bool isCannonBallRetrieved;

    public Vector2 spawnMin;
    public Vector2 spawnMax;

    public override void InitPuzzle()
    {
        isCannonBallRetrieved = false;

        float randomX = Random.Range(spawnMin.x, spawnMax.x);
        float randomY = Random.Range(spawnMin.y, spawnMax.y);
        Vector3 spawnPoint = new Vector3(randomX, randomY, 0);
        cannon = Instantiate(cannonPrefab, spawnPoint, Quaternion.identity);

        randomX = Random.Range(spawnMin.x, spawnMax.x);
        randomY = Random.Range(spawnMin.y, spawnMax.y);
        spawnPoint = new Vector3(randomX, randomY, 0);
        cannonBallItem = Instantiate(cannonBallItemPrefab, spawnPoint, Quaternion.identity);

        LoadScript();

        isActive = true;
    }

    public override void EndPuzzle()
    {
        isActive = false;
        if(cannon != null) Destroy(cannon);
        if(cannonBallItem != null) Destroy(cannonBallItem);
    }

    private void Awake()
    {
        isActive = false;
    }

    private void Update()
    {  
        if(!isActive) return;

        if(cannonBallItemScript.GetIsHit() & !isCannonBallRetrieved & cannonBallItem != null)
        { 
            isCannonBallRetrieved = true;
            Destroy(cannonBallItem);
        }

        if(cannonScript.GetIsHit() && isCannonBallRetrieved)
        {
            isCannonBallRetrieved = false;
            Shoot();
        }
    }

    private void Shoot()
    {
        cannonScript.RotateToTarget(boss.transform.position);

        GameObject firePoint = cannon.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        Vector3 spawnPoint = firePoint.transform.position;

        cannonBallProjectile = Instantiate(cannonBallProjectilePrefab, spawnPoint, Quaternion.identity);
        cannonBallProjectileScript = cannonBallProjectile.GetComponent<CannonBallProjectile>();
        cannonBallProjectileScript.StartMoving(boss.transform.position);
        if (explosionVFX != null)
        {
            ParticleSystem vfxInstance = Instantiate(explosionVFX, spawnPoint, Quaternion.identity);
            vfxInstance.Play();
            Destroy(vfxInstance.gameObject, vfxInstance.main.duration); // Auto-destroy after animation ends
        }
        else
        {
            Debug.LogWarning("No explosionVFX assigned in CannonPuzzle!");
        }
    }

    private void LoadScript()
    {
        Debug.Log("Finding Scripts...");

        if(cannon == null)
        {
            Debug.LogError("Cannot find Cannon GameObject. Be sure to assign it");
        }

        if(cannonBallItem == null)
        {
            Debug.LogError("Cannot find CannonBall Item GameObject. Be sure to assign it");
        }

        cannonBallItemScript = cannonBallItem.GetComponent<CannonBallItem>();
        if(cannonBallItemScript == null)
        {
            Debug.LogError("Cannot find CannonBall Script");
        }

        cannonScript = cannon.GetComponent<Cannon>();
        if(cannonScript == null)
        {
            Debug.LogError("Cannot find Cannon Script");
        }
    }
}
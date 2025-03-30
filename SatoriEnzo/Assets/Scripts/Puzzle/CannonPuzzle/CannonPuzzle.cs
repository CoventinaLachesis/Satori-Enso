using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    [SerializeField] ParticleSystem explosionVFX;
    [SerializeField] private AudioClip shootSound;



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
            Debug.Log("CannonBall Retrieved");
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
        FXPlayer.PlaySound(shootSound, transform.position,5f);
        GameObject firePoint = cannon.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        Vector3 spawnPoint = firePoint.transform.position;

        cannonBallProjectile = Instantiate(cannonBallProjectilePrefab, spawnPoint, Quaternion.identity);
        cannonBallProjectileScript = cannonBallProjectile.GetComponent<CannonBallProjectile>();

        cannonBallProjectileScript.StartMoving(boss.transform.position);
        FXPlayer.PlayVFX(explosionVFX, firePoint.transform.position, firePoint.transform.eulerAngles.z);

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
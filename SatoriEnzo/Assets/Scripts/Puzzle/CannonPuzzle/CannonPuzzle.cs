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
    private List<Collider2D> groundColliders = new();
    [SerializeField] private string[] groundTags = { "Ground" };
    [SerializeField] private float surfaceOffset = 0.5f;
    [SerializeField] ParticleSystem explosionVFX;
    [SerializeField] private AudioClip shootSound;



    public Vector2 spawnMin;
    public Vector2 spawnMax;

    public override void InitPuzzle()
    {
        isCannonBallRetrieved = false;

        FindGrounds();
        SpawnCannon();
        SpawnCannonBall();
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

    private void SpawnCannon()
    {
        Collider2D col = groundColliders[Random.Range(0, groundColliders.Count)];
        Bounds bounds = col.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y + surfaceOffset;
        Vector3 spawnPoint = new Vector3(x, y, 0);
        cannon = Instantiate(cannonPrefab, spawnPoint, Quaternion.identity);
    }

    private void SpawnCannonBall()
    {
        Collider2D col = groundColliders[Random.Range(0, groundColliders.Count)];
        Bounds bounds = col.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y + surfaceOffset;
        Vector3 spawnPoint = new Vector3(x, y, 0);
        cannonBallItem = Instantiate(cannonBallItemPrefab, spawnPoint, Quaternion.identity);
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

    private void FindGrounds()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            foreach (string tag in groundTags)
            {
                if (obj.CompareTag(tag) && obj.TryGetComponent(out Collider2D col))
                {
                    groundColliders.Add(col);
                }
            }
        }
    }
}
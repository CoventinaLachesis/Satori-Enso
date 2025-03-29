using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GatlingGun : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public float shootInterval = 0.2f;
    public int currentAmmo = 0;

    private float shootTimer = 0f;
    private bool isFiring = false;
    public GameObject gatlingAuraPrefab; // assign in Inspector
    public GameObject auraInstance;
    private GatlingGunPartSpawner spawner;
    private GatlingGunPuzzleManager manager;

    private void Awake() {
        spawner = FindObjectOfType<GatlingGunPartSpawner>();
        manager = FindObjectOfType<GatlingGunPuzzleManager>();
    
    }
    void ActivateGatlingGun()
    {
        auraInstance = Instantiate(gatlingAuraPrefab);
        auraInstance.transform.SetParent(player); // follow player
        auraInstance.transform.localPosition = new Vector3(0, 1.5f, 0); // hover above
    }

    public void StartFiring()
    {
        isFiring = true;
        currentAmmo = 10;
        if (gatlingAuraPrefab != null && auraInstance == null) { ActivateGatlingGun(); }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
    }

    void Update()
    {
        if (!isFiring || player == null ) return;

        transform.position = player.position + Vector3.up * 1.5f;

        if (currentAmmo > 0)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootInterval;
            }
        }
        else
        {
            if (auraInstance != null)
            {
                Destroy(auraInstance);
                auraInstance = null;
            }
            isFiring = false;
            //spawner.SpawnParts();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10f;
        currentAmmo--;
    }

    public void Deactive() {
        if (auraInstance != null)
        {
            Destroy(auraInstance);
            auraInstance = null;
        }
        isFiring = false;
        currentAmmo = 0;
        //spawner.SpawnParts();

    }
}
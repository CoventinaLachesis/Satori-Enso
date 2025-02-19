using UnityEngine;
using System.Collections;

public abstract class BossPattern : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    [SerializeField] public int maxHealth = 100;
    private int currentHealth;


    protected bool isShooting = false; // Prevents shooting until triggered

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected abstract IEnumerator AttackPattern();



    public void StartShooting() // Call this method when you want the boss to start shooting
    {
        if (!isShooting)
        {
            isShooting = true;
            StartCoroutine(AttackPattern());
        }
    }

    protected void ShootBullet(float angle, float speed)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BossBullet bulletScript = bullet.GetComponent<BossBullet>();
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        bulletScript.SetDirection(direction);
        bulletScript.speed = speed;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= maxHealth / 2) // Start shooting if health is below 50%
        {
            StartShooting();
        }
    }
}


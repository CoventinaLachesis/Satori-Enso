using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class BossPattern : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    
    protected AudioSource audioSource;
    private int currentHealth;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] protected Image healthBar;
    protected HealthBar healthBarScript;

    protected bool isShooting = false; // Prevents shooting until triggered

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("⚠️ No AudioSource found on " + gameObject.name + ". Adding one now.");
            audioSource = gameObject.AddComponent<AudioSource>(); // Add one automatically
        }

        // Final check to confirm the AudioSource exists
        if (audioSource == null)
        {
            Debug.LogError("❌ Failed to add AudioSource on " + gameObject.name);
        }
        else
        {
            Debug.Log("✅ AudioSource successfully assigned on " + gameObject.name);
        }

        currentHealth = maxHealth;

        if (healthBar == null)
        {
            Debug.LogError("No Health Bar Assigned to Boss");
        }
        else
        {
            healthBarScript = healthBar.GetComponent<HealthBar>();
            healthBarScript.SetMaxValue(maxHealth);
            healthBarScript.SetCurrentValue(currentHealth);
        }
    }
    protected virtual void Start() 
    {
        
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

    protected void ShootBullet(float angle, float speed, BulletMotionType motionType, float waveFreq = 5f, float waveAmp = 0.5f)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab is missing! Assign it in the Inspector.");
            return;
        }
        if (firePoint == null)
        {
            Debug.LogError("firePoint is missing! Assign it in the Inspector.");
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BossBullet bulletScript = bullet.GetComponent<BossBullet>();

        if (bulletScript != null)
        {
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            bulletScript.SetDirection(direction);
            bulletScript.speed = speed;
            bulletScript.motionType = motionType;
            bulletScript.waveFrequency = waveFreq;
            bulletScript.waveAmplitude = waveAmp;
        }
        else
        {
            Debug.LogError("Bullet prefab does not have a BossBullet script!");
        }
        PlaySound(shootSound);


    }
    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError("❌ AudioSource is NULL on " + gameObject.name);
            return;
        }

        if (clip == null)
        {
            Debug.LogError("❌ shootSound is NULL on " + gameObject.name);
            return;
        }

        //Debug.Log("🎵 Playing sound: " + clip.name);
        audioSource.PlayOneShot(clip);
    }

    public void TakeDamage(int damage)
    {
        Debug.LogError("Hit " + gameObject.name);
        currentHealth -= damage;
        Debug.Log("Hp:" + currentHealth);
        healthBarScript.SetCurrentValue(currentHealth);

    }
}


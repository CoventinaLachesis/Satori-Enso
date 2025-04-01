using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management


public abstract class BossPattern : MonoBehaviour
{
    public string nextStage = "Menu";
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] protected Image healthBar;
    [SerializeField] private AudioClip shootSound;

    [SerializeField] private float baseShakeMagnitude = 0.1f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private Transform shakeTarget;
    private Coroutine shakeCoroutine;
    private Vector3 originalPos;

    protected AudioSource audioSource;
    private float currentHealth;
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
        if (shakeTarget == null) shakeTarget = transform;
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

    public void TakeDamage(float damage)
    {
        Debug.LogError("Hit " + gameObject.name);
        currentHealth -= damage;
        Debug.Log("Hp:" + currentHealth);
        healthBarScript.SetCurrentValue(currentHealth);
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeDamageEffect(damage));

        if (currentHealth <= 0) {
            Invoke(nameof(Death), 0.5f); // Delay scene transition
        }

    }

    private void Death() {
        SceneManager.LoadScene(nextStage);
    }

    private IEnumerator ShakeDamageEffect(float damage)
    {
        float elapsed = 0f;
        float magnitude = baseShakeMagnitude * damage;

        Vector3 shakeOrigin = shakeTarget.localPosition;

        while (elapsed < shakeDuration)
        {
            Debug.Log("Shake");
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            shakeTarget.localPosition = shakeOrigin + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeTarget.localPosition = shakeOrigin;
    }

}


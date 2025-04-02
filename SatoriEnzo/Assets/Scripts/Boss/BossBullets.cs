using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 5f;
    public float lifetime = 10f;
    public int maxBounces = 5;
    public BulletMotionType motionType = BulletMotionType.Straight;

    [HideInInspector] public float waveFrequency = 5f;
    [HideInInspector] public float waveAmplitude = 0.5f;

    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 basePosition;
    private Vector2 perpendicular;
    private float timeAlive = 0f;
    private int bounceCount = 0;
    private bool initialized = false;
    public float bounceMultiplier = 1.2f;
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        basePosition = transform.position;
        perpendicular = new Vector2(-direction.y, direction.x);
        initialized = true;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!initialized)
        {
            Debug.LogWarning("BossBullet direction not set. Defaulting to Vector2.right.");
            SetDirection(Vector2.right);
        }

        if (motionType == BulletMotionType.Straight)
        {
            rb.velocity = direction * speed;
        }

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        if (motionType == BulletMotionType.Straight)
            return;

        ApplyScriptedMotion();
    }

    private void ApplyScriptedMotion()
    {
        float offset = 0f;

        if (motionType == BulletMotionType.SineWave)
        {
            offset = Mathf.Sin(timeAlive * waveFrequency) * waveAmplitude;
        }
        else if (motionType == BulletMotionType.Zigzag)
        {
            offset = Mathf.Sign(Mathf.Sin(timeAlive * waveFrequency)) * waveAmplitude;
        }

        Vector2 forward = direction * speed * Time.deltaTime;
        Vector2 side = perpendicular * offset;
        Vector2 newPos = rb.position + forward + side * Time.deltaTime * waveFrequency;

        rb.MovePosition(newPos);
    }


}

using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    public float bounceMultiplier = 1.2f;
    public float extraBounceForce = 5f;
    public int maxBounces = 3;

    private Rigidbody2D rb;
    private int bounceCount = 0;
    private Vector2 direction;
    private Collider2D bulletCollider;
    private bool isCollisionEnabled = false; // Tracks when collision should be allowed

    public BulletMotionType motionType = BulletMotionType.Straight;

    // For sine wave motion
    [HideInInspector] public float waveFrequency = 5f;
    [HideInInspector] public float waveAmplitude = 0.5f;
    private Vector2 perpendicular;
    private Vector2 basePosition;
    private float timeAlive;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        basePosition = transform.position;
        perpendicular = new Vector2(-direction.y, direction.x); // for wave offset

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();

        // Disable collision with other bullets on spawn
        bulletCollider.enabled = false;
        Invoke(nameof(EnableCollision), 0.5f);

        rb.velocity = direction * speed;

        if (gameObject.scene.rootCount != 0)
        {
            Destroy(gameObject, lifetime);
        }
    }

    void EnableCollision()
    {
        bulletCollider.enabled = true; // Enable collision after 0.5s
        isCollisionEnabled = true;
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        Vector2 moveDir = direction;
        Vector2 newPos = transform.position;

        switch (motionType)
        {
            case BulletMotionType.Straight:
                break;

            case BulletMotionType.SineWave:
                float sineOffset = Mathf.Sin(timeAlive * waveFrequency) * waveAmplitude;
                newPos = basePosition + direction * speed * timeAlive + perpendicular * sineOffset;
                rb.MovePosition(newPos);
                break;

            case BulletMotionType.Zigzag:
                float zigzagOffset = Mathf.Sign(Mathf.Sin(timeAlive * waveFrequency)) * waveAmplitude;
                newPos = basePosition + direction * speed * timeAlive + perpendicular * zigzagOffset;
                rb.MovePosition(newPos);
                break;

            case BulletMotionType.Custom:
                // you can add your own motion pattern here
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCollisionEnabled) return; // Ignore collision during the first 0.5s

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bullet"))
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, normal) * bounceMultiplier;

            rb.velocity = reflectedVelocity;
            rb.AddForce(reflectedVelocity.normalized * extraBounceForce, ForceMode2D.Impulse);

            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
            }
        }
    }
}

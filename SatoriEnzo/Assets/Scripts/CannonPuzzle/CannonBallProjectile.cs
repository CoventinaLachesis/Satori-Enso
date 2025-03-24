using System.Collections;
using UnityEngine;

public class CannonBallProjectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10f;

    [Header("VFX & Sound")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private ParticleSystem explosionVFX;

    private Rigidbody2D body;
    private bool hasHit = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

    }

    public void StartMoving(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        body.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Boss"))
        {
            hasHit = true;

            Vector3 hitPoint = transform.position;
            FXPlayer.PlayVFX(explosionVFX, transform.position);
            FXPlayer.PlaySound(hitSound, transform.position,5f);

            BossPattern bossScript = other.GetComponent<BossPattern>();
            if (bossScript != null)
            {
                bossScript.TakeDamage(20);
            }

            Destroy(gameObject);
        }
    }

}

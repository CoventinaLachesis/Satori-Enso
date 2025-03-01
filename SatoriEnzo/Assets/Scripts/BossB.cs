using System.Collections;
using UnityEngine;

public class BossTypeB : BossPattern
{
    protected override void Start()
    {
        Invoke(nameof(InitializeBoss), 1f); // Waits 1 second to ensure everything loads
    }

    private void InitializeBoss()
    {
        if (audioSource == null)
        {
            Debug.LogError("❌ AudioSource is still NULL on " + gameObject.name);
        }

        StartShooting(); // Only starts shooting if AudioSource is ready
    }


    protected override IEnumerator AttackPattern()
    {
        while (true)
        {
            for (int i = 0; i < 16; i++)
            {
                float angle = i * (360f / 16);
                ShootBullet(angle, 6f);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Replace "Player" with the actual tag of your player character
        {
            StartShooting();
        }
    }
}

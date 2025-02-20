using System.Collections;
using UnityEngine;

public class BossTypeB : BossPattern
{
    protected override void Start()
    {
        Invoke(nameof(StartShooting), 3f); // Waits 3 seconds, then starts shooting
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

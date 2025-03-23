using System.Collections;
using UnityEngine;

public class BossTypeA : BossPattern
{
    protected override void Start()
    {
        Invoke(nameof(StartShooting), 3f); // Waits 3 seconds, then starts shooting
    }


    protected override IEnumerator AttackPattern()
    {
        float angle = 0f;
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                float currentAngle = angle + (i * 36f);
                ShootBullet(currentAngle, 5f);
            }
            angle += 10f;
            yield return new WaitForSeconds(fireRate);
        }
    }
}

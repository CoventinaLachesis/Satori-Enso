using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Bullet Patterns/Straight Shoot")]

public class StraightShootingPattern : BulletPatternSO
{

    public float bulletFrequency = 1f;
    public ShootDirection shootDirection = ShootDirection.Down;

    public override IEnumerator Execute(Transform firePoint, BossPattern boss)
    {
        float baseAngle = GetAngleFromDirection(shootDirection);
        for (int i = 0; i < bulletCount; i++)

            {
                ShootBullet(firePoint, baseAngle);
            }
            yield return new WaitForSeconds(1/bulletFrequency);
        
    }


#if UNITY_EDITOR
    public override void DrawGizmos(Transform firePoint)
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = GetAngleFromDirection(shootDirection);
            if (motionType == BulletMotionType.SineWave)
            {
                DrawSineWavePath(firePoint.position, angle, waveAmplitude, waveFrequency, speed);
            }
            else
            {
                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right * 2f;
                Gizmos.DrawRay(firePoint.position, dir);
            }
        }
    }
#endif
}
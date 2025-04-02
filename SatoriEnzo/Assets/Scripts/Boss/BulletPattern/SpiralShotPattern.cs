using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// SpiralShotPattern.cs
[CreateAssetMenu(menuName = "Bullet Patterns/Spiral Shot")]
public class SpiralShotPattern : BulletPatternSO
{
    public int spirals = 3;
    public float interval = 0.1f;

    public override IEnumerator Execute(Transform firePoint, BossPattern boss)
    {
        float angle = 0;
        for (int i = 0; i < spirals * 20; i++)
        {
            ShootBullet(firePoint, angle);
            angle += 18f;
            yield return new WaitForSeconds(interval);
        }
    }

#if UNITY_EDITOR
public override void DrawGizmos(Transform firePoint)
{
    Gizmos.color = Color.magenta;
    float angle = 0;
    for (int i = 0; i < spirals * 5; i++)
    {
        angle += 18f;
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
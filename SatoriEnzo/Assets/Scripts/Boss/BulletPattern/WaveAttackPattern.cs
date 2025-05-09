using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// WaveAttackPattern.cs
[CreateAssetMenu(menuName = "Bullet Patterns/Wave Attack")]
public class WaveAttackPattern : BulletPatternSO
{
    public ShootDirection shootDirection = ShootDirection.Down;
    public float interval = 0.2f;

    public override IEnumerator Execute(Transform firePoint, BossPattern boss)
    {
        float baseAngle = GetAngleFromDirection(shootDirection);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = baseAngle + Mathf.Sin(Time.time * 5f) * 30f;
            ShootBullet(firePoint, angle);
            yield return new WaitForSeconds(interval);
        }
    }




#if UNITY_EDITOR
public override void DrawGizmos(Transform firePoint)
{
    Gizmos.color = Color.cyan;
    float baseAngle = GetAngleFromDirection(shootDirection);

    for (int i = 0; i < bulletCount; i++)
    {
        float angle = baseAngle + Mathf.Sin(i * 0.5f) * 30f;
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
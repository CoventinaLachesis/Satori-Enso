using System.Collections;
using UnityEngine;

public class BossTypeB : BossPattern
{
    [SerializeField] private BulletPatternSO[] patterns;

    protected override IEnumerator AttackPattern()
    {
        while (true)
        {
            foreach (var pattern in patterns)
            {
                yield return pattern.Execute(firePoint, this);
            }
        }
    }
    protected override void Start()
    {
        Invoke(nameof(InitializeBoss), 1f);
    }

    private void InitializeBoss()
    {
        if (audioSource == null)
        {
            Debug.LogError($"❌ AudioSource is NULL on {gameObject.name}");
        }

        if (patterns != null && patterns.Length > 0)
        {
            StartShooting();
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (var pattern in patterns)
        {
            pattern?.DrawGizmos(firePoint);
        }
    }
    #endif

}

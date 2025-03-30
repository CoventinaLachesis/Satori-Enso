using UnityEngine;

public static class FXPlayer
{
    public static void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null || AudioPool.Instance == null) return;

        AudioPool.Instance.PlayOneShot(clip, position, volume);
    }

    public static void PlayVFX(ParticleSystem vfxPrefab, Vector3 position, float rotationZ = 0f)
    {
        if (vfxPrefab == null) return;

        Quaternion rot = Quaternion.Euler(0, 0, rotationZ);
        ParticleSystem vfx = Object.Instantiate(vfxPrefab, position, rot);
        vfx.Play();

        Object.Destroy(vfx.gameObject, vfx.main.duration);
    }
}


using UnityEngine;
using System.Collections.Generic;

public class AudioPool : MonoBehaviour
{
    public static AudioPool Instance;

    [SerializeField] private int maxSources = 32;
    private List<AudioSource> sources;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        sources = new List<AudioSource>();

        for (int i = 0; i < maxSources; i++)
        {
            GameObject go = new GameObject("PooledAudioSource_" + i);
            go.transform.parent = transform;

            AudioSource src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;

            sources.Add(src);
        }
    }

    public void PlayOneShot(AudioClip clip, Vector3 pos, float volume = 1f)
    {
        foreach (AudioSource src in sources)
        {
            if (!src.isPlaying)
            {
                src.transform.position = pos;
                src.volume = volume;
                src.spatialBlend = 0f; // 2D sound
                src.PlayOneShot(clip);
                return;
            }
        }

        // All sources busy — optional: force one to play
        Debug.LogWarning("All audio sources busy. Consider increasing pool size.");
    }
}

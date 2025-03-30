using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance;
    public AudioSource audioSource;

    // List of scenes where music should keep playing
    public string[] allowedScenes;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep music across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the current scene is not in the allowed list, stop the music
        if (!IsSceneAllowed(scene.name))
        {
            Destroy(gameObject);
        }
    }

    private bool IsSceneAllowed(string sceneName)
    {
        foreach (string allowedScene in allowedScenes)
        {
            if (sceneName == allowedScene)
            {
                return true;
            }
        }
        return false;
    }
}

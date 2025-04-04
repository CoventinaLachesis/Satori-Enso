using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    // Assign the name of the scene you want to load in the Unity Editor.
    public string sceneName;

    // Optional: Assign the button through the Inspector.
    public Button startButton;

    void Start()
    {
        // Check if the button is assigned. If not, try to find it automatically.
        if (startButton == null)
        {
            startButton = GetComponent<Button>();
        }

        // Add a listener to the button to call the StartGame method when clicked.
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("Start button is not assigned!");
        }
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)){ 
            StartGame();
        }
    }

    void StartGame()
    {
        // Check if the scene name is valid before attempting to load it.
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not set or is empty!");
        }
    }
}

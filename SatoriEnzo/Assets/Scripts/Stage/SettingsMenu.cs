using UnityEngine;
using UnityEngine.UI;
using TMPro;  // If using TextMeshPro

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Dropdown displaySizeDropdown;
    public Slider volumeSlider;

    private Resolution[] resolutions;

    void Start()
    {
        // Initialize resolution options
        resolutions = Screen.resolutions;
        displaySizeDropdown.ClearOptions();

        // Populate dropdown with available resolutions
        foreach (Resolution resolution in resolutions)
        {
            displaySizeDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.width + "x" + resolution.height));
        }

        // Load saved settings
        LoadSettings();
    }

    public void ApplySettings()
    {
        // Apply resolution
        int selectedResolutionIndex = displaySizeDropdown.value;
        Resolution selectedResolution = resolutions[selectedResolutionIndex];
        // Log the resolution being set
        Debug.Log($"Applying Resolution: {selectedResolution.width}x{selectedResolution.height}");

        // Set resolution and window mode
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, FullScreenMode.Windowed);  // Change to FullScreenMode.MaximizedWindow or Windowed

        // Apply volume
        float volume = volumeSlider.value;
        AudioListener.volume = volume;

        // Save settings
        PlayerPrefs.SetInt("ResolutionIndex", selectedResolutionIndex);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }


    private void LoadSettings()
    {
        // Load resolution
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        displaySizeDropdown.value = savedResolutionIndex;
        displaySizeDropdown.RefreshShownValue();

        // Load volume
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        volumeSlider.value = savedVolume;
    }
}

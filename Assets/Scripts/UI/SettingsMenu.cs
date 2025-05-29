using UnityEngine;
// Sets the values of settings that the player inputs in the settings menu
public class SettingsMenu : MonoBehaviour {
    // Save load along with getting the Loaded instance of the settings
    private LoadSettings settingsLoadInstance;
    private Settings settings;
    private string settingsFileName;

    // Loads the instance of the settings into a script-local variable
    // Input: None
    // Output: None
    private void Start() {
        // Existence check to ensure the settings script is loaded
        if(LoadSettings.settingsLoaderInstance != null){
            settingsLoadInstance = LoadSettings.settingsLoaderInstance;
            settingsFileName = settingsLoadInstance.settingsFileName;
            settings = settingsLoadInstance.settings;
        }
        else{
            Debug.LogError("ERROR! Settings save/load isn't loaded");
        }

    }

    // Invoked from the quality drop down UI, updating the quality index based on selection
    // Input: The quality the user selected from the drop down
    // Output: Saved settings change to file
    public void SetQuality(int qualityIndex){
        // Range check to ensure that quality never exceeds the intended setting amount
        qualityIndex = Mathf.Clamp(qualityIndex,0,4);
        QualitySettings.SetQualityLevel(qualityIndex);
        settings.qualityIndex = qualityIndex;
        SaveLoad.SaveSettings(settings,settingsFileName);
    }

    // Alters screen state from checkbox UI element and saves decision
    // Input: The fullscreen state
    // Output: The change made to the settings and save to file
    public void ToggleFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
        settings.isFullscreen = isFullscreen;
        SaveLoad.SaveSettings(settings,settingsFileName);
    }

    // User selects the resolution they want from dropdown taht has resolutions loaded
    // Input: The selected resolution
    // Output: Saved setting change to file
    public void SetResolution(int resolutionIndex){
        // Range check prevents selecting higher resolution than is supported by monitor
        resolutionIndex = Mathf.Clamp(resolutionIndex,0,Screen.resolutions.Length-1);
        Resolution selectedResolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width,selectedResolution.height,Screen.fullScreen);
        settings.resolutionIndex = resolutionIndex;
        SaveLoad.SaveSettings(settings,settingsFileName);
    }

    // The volume the user wants based on slider position
    // Input: The volume between altered negative decibels to same created volume
    // Output: Saved the volume to settings file
    public void SetVolume(float volume){
        // Range check to ensure volume is within intended range
        volume = Mathf.Clamp(volume,-80f,0f);
        settingsLoadInstance.mainVolume.SetFloat("Volume",volume);
        settings.volume = volume;
        SaveLoad.SaveSettings(settings,settingsFileName);
    }
}
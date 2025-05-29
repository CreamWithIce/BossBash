using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEditor;
// Loads in the settings at the start of the game and applies the read values
public class LoadSettings : MonoBehaviour {
    // Settings that are loaded and referenced
    public static LoadSettings settingsLoaderInstance;
    public string settingsFileName = "settings.json";
    public Settings settings;

    // UI elements that require updating for settings
    public Resolution[] supportedResolutions;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public AudioMixer mainVolume;
    public Slider volumeSlider;
    [SerializeField] private Toggle fullscreenToggle;

    // Updates the settings to the game corespondingly
    // Input: None
    // Output: None
    private void Start() {
        // Sets the instance of the script to be used by other scripts
        if(settingsLoaderInstance == null){
            settingsLoaderInstance = this;
        }
        // Loads in  the settings from the file
        settings = SaveLoad.LoadGameSettings(settingsFileName);
        GetResolutions();
        // Existence check to check if the file exists and creates a new settings file if not to correlate to file security
        if(settings != null){
            UpdateQuality(settings);
            UpdateResolution(settings);
            UpdateFullscreenToggle(settings);
            UpdateVolume(settings);
        }
        else{
            settings = new Settings();
            settings.volume = 0;
            settings.qualityIndex = 0;
            settings.resolutionIndex = 0;
            settings.isFullscreen = true;
            UpdateQuality(settings);
            UpdateResolution(settings);
            UpdateFullscreenToggle(settings);
            UpdateVolume(settings);
            SaveLoad.SaveSettings(settings,settingsFileName);
        }
    }

    // Gets all the resolutions supported by the screen by iterating and adding all to a drop down
    // Input: None
    // Output: None
    private void GetResolutions(){
        supportedResolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        resolutionDropdown.ClearOptions();
        List<string> resolutionDetails = new List<string>();
        for(int i = 0; i < supportedResolutions.Length; i++){
            string widthHeightDetails = supportedResolutions[i].width + " x " + supportedResolutions[i].height;

            if(supportedResolutions[i].width == Screen.width 
                && supportedResolutions[i].height == Screen.height){
                    currentResolutionIndex = i;
            }
            resolutionDetails.Add(widthHeightDetails);
        }
        resolutionDropdown.AddOptions(resolutionDetails);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    // Updates the resolution the game should be to match the settings
    // Input: The settings that are loaded or created
    // Output: None
    private void UpdateResolution(Settings settings){
        // Range check to ensure that the settings resolution is within the bounds of screen resolution
        settings.resolutionIndex = Mathf.Clamp(settings.resolutionIndex,0,supportedResolutions.Length-1);
        Resolution selectedResolution = supportedResolutions[settings.resolutionIndex];
        Screen.SetResolution(selectedResolution.width,selectedResolution.width,settings.isFullscreen);
        resolutionDropdown.SetValueWithoutNotify(settings.resolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    // Updates the quality of textures and shadows within the unity project and updates the drop down accordingly
    // Input: Settings loaded or created
    // Output: None
    private void UpdateQuality(Settings settings){
        // Range check ensures the quality setting is within the unity quality settings bounds
        settings.qualityIndex = Mathf.Clamp(settings.qualityIndex,0,4);
        QualitySettings.SetQualityLevel(settings.qualityIndex);
        qualityDropdown.SetValueWithoutNotify(settings.qualityIndex);
        qualityDropdown.RefreshShownValue();
    }

    // Sets the screen to be fullscreen or not and updates UI elements accordingly
    // Input: Settings that are loaded or created
    // Output: None
    private void UpdateFullscreenToggle(Settings settings){
        fullscreenToggle.SetIsOnWithoutNotify(settings.isFullscreen);
        Screen.fullScreen = settings.isFullscreen;
    }

    // Updates the volume in the game based on the settings value as well as the UI slider
    // Input: The settings that are loaded or created
    // Output: None
    private void UpdateVolume(Settings settings){
        // Range check ensures that volume is within the safe decibel range when loading
        settings.volume = Mathf.Clamp(settings.volume,-80f,0f);
        mainVolume.SetFloat("Volume",settings.volume);
        volumeSlider.SetValueWithoutNotify(settings.volume);
    }
}

// This class is responsible for loading and saving the files to json formats
public static class SaveLoad{
    // Converts data to a json file format and writes the data to the file
    // Input: Settings and the file name ot save data to
    // Output: Settings json file with the desired settings values
    public static void SaveSettings(Settings settingsData,string fileName){
        string jsonFormatSettings = JsonUtility.ToJson(settingsData);
        File.WriteAllText(fileName,jsonFormatSettings);
    }

    // Converts the json format data back into usable data of the settings data structure
    // Input: The settings file name
    // Output: The settings that were saved
    public static Settings LoadGameSettings(string fileName){
        // Catches any error loading file and returns a null state if there are problems
        try{
            string settingsJsonFormat = File.ReadAllText(fileName);
            Settings settingsConfiguration = JsonUtility.FromJson<Settings>(settingsJsonFormat);
            return settingsConfiguration;
        }
        catch{
            Debug.LogError("ERROR! Settings file damaged or non-existent");
            return null;
        }
    }
}

// The settings data structure that is able to be saved as it is encoded to a standard format
[System.Serializable]
public class Settings{
    public float volume;
    public int qualityIndex;
    public int resolutionIndex;
    public bool isFullscreen;
}
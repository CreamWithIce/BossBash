using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Diagnostics;
// All main menu buttons are altered through here
public class MainMenu : MonoBehaviour {
    // Canvas' that need to be hidden or shown depending which menu the player is in
    [SerializeField] private GameObject settingsMenuCanvas;
    [SerializeField] private GameObject mainMenuCanvas;

    // The button that is selected on different screens
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject settingsItemSelected;

    // Loading screen to be activated or hidden to the player
    [SerializeField] private GameObject loadingScreen;

    // At the start we ensure that the settings menu is definitely closed on the main screen to avoid confusion with the UI
    // Input: None 
    // Output: None
    private void Start() {
        CloseSettingsMenu();
    }

    // When the play button is pressed, it will load the game scene and enable the load screen
    // Input: None
    // Output: None
    public void PlayGame(){
        loadingScreen.SetActive(true);
        DontDestroyOnLoad(loadingScreen);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        
    }
    
    // Would quit the application if the game was built to launch and the quit button pressed
    // Input: None
    // Output: None
    public void Quit(){
        Application.Quit();
    }

    // Changes canvas' so the player can navigate the menu settings
    // Input: None
    // Output: None
     public void OpenSettingsMenu(){
        settingsMenuCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(settingsItemSelected);
    }

    // Whenever the resume button is pressed or main menu scene is entered, it makes the main menu visible
    // Input: None
    // Output: None
    public void CloseSettingsMenu(){
        settingsMenuCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(playButton);
    }
}
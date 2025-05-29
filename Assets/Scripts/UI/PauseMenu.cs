using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
// Handles pausing the game as well as button events for settings and resuming the game
public class PauseMenu : MonoBehaviour {
    // Menu UI's and elements that are going to be selected first for navigation purposes
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject settingsItemFirstSelect;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseItemFirstSelect;

    // References to the game and player specific elements
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject player;

    // Other auxileray menus and UI that indicate other game status'
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject winScreen;

    // Scripts to control player based on game paused or not
    private PlayerMovement movement;
    private FireProjectile projectileLaunch;
    private InputLoader inputs;

    // Keeps track of the game state is it is paused or not
    private bool isGamePaused = false;
    // Loads inputs and player scripts, performing existence checks to ensure the scripts and inputs are loaded
    // Input: None
    // Output: None
    private void Start() {
        if(InputLoader.inputInstance != null){
            inputs = InputLoader.inputInstance;
        }
        else{
            Debug.LogError("ERROR! Inputs not loaded!");
        }

        movement = player.GetComponent<PlayerMovement>();
        projectileLaunch = player.GetComponent<FireProjectile>();
    }

    // Pauses and unpauses the game if the appropriate input is pressed
    // Input: None
    // Output: None
    private void Update() {
        if(inputs.pauseGame.WasPressedThisFrame()){
            // Range check to change game state
            if(isGamePaused == false){
                PauseGame();
            }
            else if(isGamePaused == true){
                UnpauseGame();
            }
        }
    }

    // Opens settings if the settings button is pressed in the pause menu
    // Input: None
    // Output: None
    public void OpenSettingsMenu(){
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsItemFirstSelect);
    }

    // Goes back to pause menu if back button in settings menu is pressed
    // Input: None
    // Output: None
    public void OpenPauseMenu(){
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        playerUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(pauseItemFirstSelect);
    }

    // If resume button or unpause action is taken then all menus should close
    // Input: None
    // Output: None
    public void CloseMenus(){
        playerUI.SetActive(true);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    // If the player wants to go to main menu it will load the scene and put up loading screen, resetting settings in the process
    // Input: None
    // Output: None
    public void GoToMainMenu(){
        UnpauseGame();
        Scene currentScene = SceneManager.GetActiveScene();
        Scene mainMenu = SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex-1);
        Time.timeScale = 1f;
        // Existence check that the main menu does exist in the scene settings
        if(mainMenu!=null){
            playerUI.SetActive(false);
            winScreen.SetActive(false);
            loadingScreen.SetActive(true);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex-1);
            SceneManager.UnloadSceneAsync(currentScene.buildIndex);
        }
        else{
            Debug.LogError("Main menu does not exist!");
        }
    }

    // Blocks inputs from being turned into action and stops the time passing in game when paused
    // Input: None
    // Output: None
    private void PauseGame(){
        Time.timeScale = 0f;
        isGamePaused = true;
        movement.enabled = false;
        projectileLaunch.enabled = false;
        OpenPauseMenu();
    }

    // Reverts alterations that were done when the game was paused
    // Input: None
    // Output: None
    public void UnpauseGame(){
        Time.timeScale = 1f;
        isGamePaused = false;
        movement.enabled = true;
        projectileLaunch.enabled = true;
        CloseMenus();
    }
}
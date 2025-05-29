using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// Handles resetting the game state or advancing it based on certain events
public class GameManager : MonoBehaviour {
    // To reset player position when called to reset
    [SerializeField] private Transform player;

    // Settings and variables required to select and load training bots and train the AI
    [SerializeField] private List<RunBot> trainingBots = new List<RunBot>();
    [SerializeField] private List<string> botFiles = new List<string>();
    [SerializeField] private bool training = false;
    System.Random randomGenerator = new System.Random();

    // Health to ensure that health values are reset correctly when a game reset is performed
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private BossHealth bossHealth;

    // Player UI elements that are set active or hidden based on the event performed
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject playerHud;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject replayButtonSelected;

    // Resets the game back to its default state whenever the player is defeated to continue playing as usual
    // Inputs: None
    // Outputs: None
     public void ResetSystems(){
        // Range check to ensure that training bots are only reset if there is training happening
        if(training == true){
            foreach(RunBot trainingBot in trainingBots){
                trainingBot.inputFrame = 0;
                int botFileIndex = randomGenerator.Next(botFiles.Count);
                trainingBot.ReadFile(botFiles[botFileIndex]);
                trainingBot.gameObject.transform.localPosition = new Vector3(-5.35f,4.88f,0f);
            }
        }
        // Resets UI elements, health values and player data back to default
        if(playerHealth!=null){
            playerHealth.ResetHealth();
            bossHealth.ResetHealth();
            boss.SetActive(true);
            playerHud.SetActive(true);
            winScreen.SetActive(false);
            menuCanvas.GetComponent<PauseMenu>().enabled = true;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
            
        
        player.localPosition = new Vector3(-5.35f,4.88f,0f);
        Time.timeScale = 1f;
        EventSystem.current.SetSelectedGameObject(null);
        }
       
    }

    // Opens the win screen for when the boss is defeated
    // Inputs: None
    // Outputs: None
    public void GameWin(){
        menuCanvas.GetComponent<PauseMenu>().enabled = false;
        boss.SetActive(false);
        playerHud.SetActive(false);
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(replayButtonSelected);
    }

    
}
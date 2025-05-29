using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
// Keeps track of the countdown timer that is used to reset the game state
public class Timer : MonoBehaviour {
    // Timer related UI and set values
    [SerializeField] float maxTimeSeconds = 60;
    [SerializeField] TMP_Text timerText;
    
    // To be able to reset settings by invoking the end of an AI session
    private BossMLAgent bossAI;
    // Keeps track of the current time
    private float timeSeconds;

    // Resets values needed and gets the AI script
    // Input: None
    // Output: None
    private void Start() {
        timeSeconds = maxTimeSeconds;
        // Existence check the AI has been loaded
        if(BossMLAgent.bossAI!=null){
            bossAI = BossMLAgent.bossAI;
        }
        else{
            Debug.LogError("Error! Could not instantiate boss AI, timer script");
        }
        ResetTimer();
    }

    // Able to reset the timer when resetting the game
    // Input: None
    // Output: None
    public void ResetTimer(){
        timeSeconds = maxTimeSeconds;
    }

    // Updates the timer every frame and the UI element
    // Input: None
    // Output: None
    private void Update() {
        // Range check to ensure timer does not become negative and resets time
        if(timeSeconds <= 0){
            bossAI.EndEpisode();
            RecordTrainingBots.botRecording.SaveResults();
        }
        timeSeconds -= Time.deltaTime;
        timerText.text = Mathf.RoundToInt(timeSeconds).ToString();

    }
    
}
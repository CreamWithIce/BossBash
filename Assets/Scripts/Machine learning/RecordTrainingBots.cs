using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
// Records user inputs for playback during the training process
public class RecordTrainingBots : MonoBehaviour {
    // External references to make the player record properly
    private InputLoader inputs;
    [SerializeField] private Rigidbody2D playerRb;

    // File save settings
    [SerializeField] private bool currentlyRecording;
    [SerializeField] private string savePath = @".\Assets\";
    [SerializeField] private string fileName = @"circle.txt";

    // Buffer that inputs for velocity is saved to
    private List<string> inputData = new List<string>();

    // Exposes instance of class to global scope
    public static RecordTrainingBots botRecording;
    // Initialized on startup, loads inputs and adds the class instance to global scope
    // Input: None
    // Output: None
    private void Start() {
        // Existence check to see if inputs exists
        if(InputLoader.inputInstance != null){
            inputs = InputLoader.inputInstance;
        }

        else{
            Debug.LogError("ERROR INPUTS NOT LOADED");
        }
        // Existence check to ensure that instance has not been assigned
        if(botRecording == null){
            botRecording = this;
        }
    }
    // Every fixed update (~0.02 seconds) if the inputs are being recorded, the players velocity will be stored to be saved
    // Inputs: None
    // Output: None
    private void FixedUpdate() {
        // Range check to ensure that recording is happening to save the player velocity
        if(currentlyRecording == true){
            AppendInputs(playerRb.velocity);
        }
    }
    
    // Saves the velocity inputs to a file that can be played back later for machine learning training
    // Input: None
    // Output: Saves the inputs to a file
    public void SaveResults(){
        if(currentlyRecording == true){
            currentlyRecording = false;
            using (StreamWriter fileWriter = File.CreateText(savePath+fileName)){
                foreach(string input in inputData){
                    fileWriter.WriteLine(input);
                }
                fileWriter.Close();
            }
        }
    }
    
    // Converts the floating point numbers to string type which is in a format to be able to be saved
    // Input: the player velocity at the current frame
    // Output: None
    private void AppendInputs(Vector2 playerVelocity){
        string xVelocity = playerVelocity.x.ToString();
        string yVelocity = playerVelocity.y.ToString();
        inputData.Add(xVelocity+","+yVelocity);
    }
}
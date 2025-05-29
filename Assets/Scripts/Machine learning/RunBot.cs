using UnityEngine;
using System.Collections.Generic;
using System;
// Used during training, plays back the recorded velocities at deterministic intervals to train the MLagent to aim
public class RunBot : MonoBehaviour {
    // External reference to move the bot
    private Rigidbody2D botRb;
    // Exposed variables to load the input velocity from files
    public string[] inputData;
    public int inputFrame = 0;

    // Runs at start of scene loading to be able to move the bot if applied
    // Input: None
    // Output: None
    private void Start() {
        botRb = GetComponent<Rigidbody2D>();
        // Existence check to ensure the rigidbody exists
        if(botRb == null){
            Debug.LogError("Error! Bots rigidbody not detected!");
        }

    }

    // Runs every 20 ms to play back inputs at the same rate as was captured, applying them to the bot
    // Input: None
    // Output: None
    private void FixedUpdate() {
        // Range check to ensure to only access within the bounds of the velocities indices
        if(inputFrame < inputData.Length-1){
            string[] individualInputs = inputData[inputFrame].Split(",");
            // Implicit type check to ensure that the velocities from the frame index are able to be set as a new velocity
            Vector2 botVelocity = new Vector2((float)Convert.ToDouble(individualInputs[0]),(float)Convert.ToDouble(individualInputs[1]));
            botRb.velocity = botVelocity;
            inputFrame++;
        }
    }

    // Reads file from the desired file that is randomly selected
    // Input: The file name to load
    // Output: The contents of the file data as a string array
    public string[] ReadFile(string inputFileName){
        TextAsset inputsFile = (TextAsset)Resources.Load(inputFileName);
        if(inputsFile == null){
            Debug.LogError("Error! Input file: " + inputFileName + " not found");
            UnityEditor.EditorApplication.ExitPlaymode();
        }
        inputData = inputsFile.text.Split("\n");
        return inputData;
    }
}
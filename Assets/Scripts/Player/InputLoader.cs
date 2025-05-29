using UnityEngine;
using UnityEngine.InputSystem;
// Loads inputs and stores them n a class that can later be referenced
public class InputLoader : MonoBehaviour {
    // Instance of the class that will be referenced in other scripts
    public static InputLoader inputInstance;
    // The object that inputs will be extracted from
    public PlayerInput inputMap;
    // References to specific inputs that are easier to access and use
    [HideInInspector] public InputAction playerMovement;
    [HideInInspector] public InputAction fireProjectile;
    [HideInInspector] public InputAction pauseGame;
    [HideInInspector] public InputAction controllerAim;

    private void Start() {
        // Validates that an instance should exist in game
        if(inputInstance==null){
            inputInstance=this;
        }
        AssignInputActions();
    }

    // Assigns input actions from a centralized script to avoid having to track inputs across many scripts
    // Input: None
    // Output: None (ignoring assigning global variables)
    void AssignInputActions(){
        playerMovement = inputMap.actions["Movement"];
        fireProjectile = inputMap.actions["Shoot"];
        pauseGame = inputMap.actions["Pause"];
        controllerAim = inputMap.actions["Aiming"];
    }

}
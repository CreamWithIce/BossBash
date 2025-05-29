using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// Converts appropriately assigned inputs to movement of the character in the game
public class PlayerMovement : MonoBehaviour
{
    // Enables the player to move
    [SerializeField] public Rigidbody2D playerRB;
    private InputLoader moveInputs;

    // Sets speed of player
    [SerializeField] private float speedScalar = 12f;



    // Loads the input map if the script has loaded through existence checking
    // Input: None
    // Output: None
    void Start()
    {
        if(InputLoader.inputInstance!=null){
            moveInputs = InputLoader.inputInstance;
        }
    }

    // Updates the player position based on movement value
    // Input: None
    // Output: None
    void Update()
    {
        // Range check to ensure inputs are correct
        Vector2 movementDirection = moveInputs.playerMovement.ReadValue<Vector2>();
        if((movementDirection.x >= -1f && movementDirection.x <= 1f)||(movementDirection.y >= -1f && movementDirection.y <= 1f)){
            playerRB.AddForce(movementDirection*speedScalar);
        }
    }
}

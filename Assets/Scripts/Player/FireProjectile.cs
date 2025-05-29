using UnityEngine;
using UnityEngine.InputSystem;
// Updates the aiming direction visually and handles the player firing projectiles
public class FireProjectile : MonoBehaviour {
    // External player components for aiming and firing projectiles
    [SerializeField] Transform playerTransform;
    [SerializeField] Camera mainCamera;
    // Visual aim indicator references
    [SerializeField] Transform aimIndicator;
    [SerializeField] Transform fireFromPoint;

    // Controls how the projectile behaves when fired
    [SerializeField] private float projectileCooldown = 0.4f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 40f;
    private bool canFireProjectile = true;

    // Controls the type of input used to aim last to keep track of where to aim to
    [SerializeField] private float controllerDeadZone = 0.4f;
    Vector3 previousMousePosition;
    private bool controllerLastUsed = false;
    Vector3 lastAimInputPosition;
    private InputLoader inputs;

    // Loads inputs at the start of the level, performing existence check to ensure it is properly loaded
    // Input: None
    // Output: None
    private void Start() {
        if(InputLoader.inputInstance!=null){
            inputs = InputLoader.inputInstance;
        }
        else{
            Debug.LogError("Error! Inputs not loaded in scene");
        }
    }

    // Runs every frame to update where the player is aiming based on inputs and fires projectile
    // Input: None
    // Output: None
    private void Update() {
        Vector2 controllerInputs = inputs.controllerAim.ReadValue<Vector2>();
    
        // Updates the last known position to aim at when an input is pressed. Otherwise it holds the position
        if(previousMousePosition != Input.mousePosition){
            controllerLastUsed = false;
            lastAimInputPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(controllerInputs.x < -controllerDeadZone || controllerInputs.x > controllerDeadZone || controllerInputs.y < -controllerDeadZone || controllerInputs.y > controllerDeadZone){
            lastAimInputPosition = controllerInputs;
            controllerLastUsed = true;
        }
        
        // Covers case where mouse is used
        Vector2 relativeAimPosition = lastAimInputPosition-playerTransform.position;
        // Covers case where controller is used (range check)
        if(controllerLastUsed == true){
            relativeAimPosition = lastAimInputPosition;
        }
        
        Vector3 aimDirection = new Vector3(aimIndicator.localRotation.x,aimIndicator.localRotation.y,CalculateProjectileAngle(relativeAimPosition));
        aimIndicator.localRotation = Quaternion.Euler(aimDirection);

        // Range checks if the input to fire is pressed and that the fire cooldown has expired
        if(canFireProjectile&&inputs.fireProjectile.WasPressedThisFrame()){
            canFireProjectile = false;
            SummonPlayerProjectile(projectilePrefab, relativeAimPosition,aimDirection);
            Invoke("FireCooldown",projectileCooldown);
        }
        previousMousePosition = Input.mousePosition;
    }

    // Uses tangent to find the angle which is being aimed at
    // In: Relative position aimed from 
    // Out: Angle from the player to the cursor
    private float CalculateProjectileAngle(Vector2 relativeAimPosition){
        float angleToPlayer = -Mathf.Rad2Deg*Mathf.Atan2(relativeAimPosition.x,relativeAimPosition.y);
        return angleToPlayer;
    }
    
    // Summons a projectile in the direction desired and speed
    // In: The projectile model, direction to fire, position to fire, whether it is a player/boss projectile and the speed at which is moves
    // Out: None
    private void SummonPlayerProjectile(GameObject projectilePrefab, Vector2 relativeAimPosition,Vector3 aimDirection){
        Quaternion rotateProjectile = Quaternion.Euler(aimDirection.x,aimDirection.y,90+aimDirection.z);
        GameObject projectileObject = Instantiate(projectilePrefab, fireFromPoint.position,rotateProjectile);
        PlayerProjectile projectileScript = projectileObject.GetComponent<PlayerProjectile>();
        Vector3 projectileMotion = relativeAimPosition.normalized*projectileSpeed;
        projectileScript.playerProjectileVelocity = projectileMotion;
    }

    // Resets projectile fire state after being invoked after a certain period of time
    // Inputs: None
    // Outputs: None
    void FireCooldown(){
        canFireProjectile = true;
    }
}
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

// Controls how the boss aims as well as ends each training episode to begin another
public class BossMLAgent : Agent {
    // External references for observations
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D playerRB;

    // Used to ensure the boss can fire projectiles
    [SerializeField] private Transform aimIndicator;
    private BossFireProjectile bossProjectile;
    private bool canFire = true;
    [SerializeField] private float projectileResetTime = 0.5f;
    [SerializeField] private LayerMask raycastLayers;

    // Exposes the class instance to the global scope
    public static BossMLAgent bossAI;

    // Reset for next training session
    [SerializeField] private Transform playerResetPoint;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Timer timer;




    // Runs at start of game, setsup several varaibles for later use
    // Input: None
    // Output: None
    private void Start() {
        bossProjectile = GetComponent<BossFireProjectile>();
        // Existence check to expose class instance to global scope
        if(bossAI == null){
            bossAI = this;
        }
    }
    // Invokes other methods to reset game to the original state the scene was loaded in
    // Input: None
    // Output: None
    public override void OnEpisodeBegin()
    {
        ClearProjectiles();
        timer.ResetTimer();
        gameManager.ResetSystems();
        
    }

    // Gets the data that the AI will need to be able to form a decision
    // Input: Sensors such as the boss position and player position as well as the option to fire
    // Output: None
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(playerTransform.localPosition);
        sensor.AddObservation(canFire);
    }

    // The action the AI thinks it should perform when it is able to perform action
    // Input: Actions the AI will perform
    // Output: None
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Outputs a float between -1 and 1
        Vector2 outputDirection = new Vector2(actions.ContinuousActions[0],actions.ContinuousActions[1]);
        // Range checks that inputs are within the range 0 to 1
        bool xOutputDirectionCheck = (-1f <= outputDirection.x && outputDirection.x <= 1f) ? true:false;
        bool yOutputDirectionCheck = (-1f <= outputDirection.y && outputDirection.y <= 1f) ? true:false;
        
        // Range check to ensure that the AI can fire a projectile at the input angle calculated
        if(canFire && xOutputDirectionCheck == true && yOutputDirectionCheck == true){
            float shootDirection = bossProjectile.CalculateProjectileAngle(outputDirection);
            Vector3 aimDirection = new Vector3(aimIndicator.localRotation.x,aimIndicator.localRotation.y,shootDirection);
            aimIndicator.localRotation = Quaternion.Euler(aimDirection);

            // Raycast provides feedback to train the AI
            RaycastHit2D ray = Physics2D.Raycast(transform.position,outputDirection,25f,raycastLayers);

            // Type/existence checks what the raycast collided with and rewards appropriately
            if(ray.collider!=null){
                if(ray.collider.gameObject.TryGetComponent<Walls>(out Walls wall)){
                    AddReward(-0.5f);
                }
                else if(ray.collider.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player)){
                    AddReward(5f);
                }
            }

            bossProjectile.SummonProjectile(outputDirection);

    
            canFire = false;
            Invoke("ProjectileCooldown", projectileResetTime);
        
        }
    }

    // Override if the user is ever needed to take control of AI
    // Input: Actions the AI would perform
    // Output: None
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    // Resets the projectile so that it can be fired again
    // Input: None
    // Output: None
    void ProjectileCooldown(){
        canFire = true;
    }

    // Destroys all projectiles in the scene before next AI session
    // Input: None
    // Output: None
    void ClearProjectiles(){
        PlayerProjectile[] playerProjectiles = (PlayerProjectile[])FindObjectsByType(typeof(PlayerProjectile),FindObjectsSortMode.None);
        for(int index = 0; index < playerProjectiles.Length; index++){
            Destroy(playerProjectiles[index].gameObject);
        }
        
        BossProjectile[] bossProjectiles = (BossProjectile[])FindObjectsByType(typeof(BossProjectile),FindObjectsSortMode.None);
        for(int index = 0; index < bossProjectiles.Length; index++){
            Destroy(bossProjectiles[index].gameObject);
        }
        
    }

}
using UnityEngine;
using System;
// When the boss projectile is spawned, this script handles the direction the projectile aims and collision that occurs
public class BossProjectile : MonoBehaviour {
    [HideInInspector] public Vector2 bossProjectileVelocity;
    [SerializeField] private float timeToLive = 0.5f;
    System.Random damageRandom = new System.Random();

    [SerializeField] private int minDamage,maxDamage;

    // References to external scripts
    private Rigidbody2D projectileRb;
    private BossMLAgent bossMLAI;

    private void Start() {
        projectileRb = GetComponent<Rigidbody2D>();
        projectileRb.velocity = bossProjectileVelocity;
        // Existance checks the BossMLAgent instance exists in scene
        if(BossMLAgent.bossAI!=null){
            bossMLAI = BossMLAgent.bossAI;
        }
        else{
            Debug.LogError("Error! Could not instantiate boss AI");
        }
        // Destroys the boss projectile after certain time so it does not cause too many to exist
        Invoke("DestroyProjectile",timeToLive);
    }

    // Type checks the object collided with to either damage the player or get destroyed
    // Input: The collider the projectile hit
    // Output: None
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.TryGetComponent<Walls>(out Walls walls)){
            DestroyProjectile();
        }
        else if(other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player)){
            int damageDealt = damageRandom.Next(minDamage,maxDamage);
            player.DamagePlayer(damageDealt);
            DestroyProjectile();
        }
    }
    
    // Destroys the projectile
    // Input: None
    // Output: None
    void DestroyProjectile(){
        Destroy(this.gameObject);
    }
}
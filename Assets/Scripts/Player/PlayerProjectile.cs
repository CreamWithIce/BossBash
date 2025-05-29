using UnityEngine;
using System;
// Is part of the projectile that is loaded into the scene when a projectile fired, handles loading and destroying the projectile
public class PlayerProjectile : MonoBehaviour {
    // Values that are referenced to set up the projectile when launched
    [HideInInspector] public Vector3 playerProjectileVelocity;
    [SerializeField] float timeToLive = 10f;

    // Damage system settings
    System.Random randomDamage = new System.Random();
    [SerializeField] private int minDamageDealt;
    [SerializeField] private int maxDamageDealt;

    // References the projectiles rigidbody to move it
    private Rigidbody2D projectileRb;

    // Sets the projectile in motion when spawned into the scene
    // Input: None
    // Output: None
    private void Start() {
        projectileRb = GetComponent<Rigidbody2D>();
        projectileRb.velocity = playerProjectileVelocity;
        SpriteRenderer projectileSprite = GetComponent<SpriteRenderer>();
       
        Invoke("DestroyProjectile",timeToLive);

    }

    // Checks collision with boss or walls
    // In: The object the projectile collided with
    // Out: None
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Type check for boss or wall to be able to damage boss
        if(other.gameObject.TryGetComponent<Walls>(out Walls wall)){
            Destroy(this.gameObject);
        }
        else if(other.gameObject.TryGetComponent<BossHealth>(out BossHealth boss)){
            int damageAmount = randomDamage.Next(minDamageDealt,maxDamageDealt);
            boss.DamageBoss(damageAmount);
            Destroy(this.gameObject);
        }
    }

    // Destroys projectile after certain amount of time via invoke call to not pile up number of projectiles
    // Input: None
    // Output: None
    void DestroyProjectile(){
        Destroy(this.gameObject);
    }

}
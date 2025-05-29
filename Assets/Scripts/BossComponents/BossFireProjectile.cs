using Unity.VisualScripting;
using UnityEngine;
// Calculations and sequences performed to spawn the projectile and figure out the angle the boss MLagent wants to fire at
public class BossFireProjectile : MonoBehaviour {
    // References to spawn projectile, placement and speed
    [SerializeField] private Transform summonPoint;
    [SerializeField] private Transform rotationTransform;
    [SerializeField] private float projectileSpeed = 30f;
    [SerializeField] private GameObject projectilePrefab;

    
    // Uses tangent to find the angle which is being aimed at
    // In: Relative position aimed from 
    // Out: Angle from the player to the cursor
    public float CalculateProjectileAngle(Vector2 relativeAimPosition){
        float angleToBoss = -Mathf.Rad2Deg*Mathf.Atan2(relativeAimPosition.x,relativeAimPosition.y);
        return angleToBoss;
    }
    // Summons a projectile in the direction desired and speed
    // In: Direction to fire
    // Out: None
    public void SummonProjectile(Vector2 relativeAimPosition){
        Quaternion projectileAngle = Quaternion.Euler(rotationTransform.localRotation.x,rotationTransform.localRotation.y,90+CalculateProjectileAngle(relativeAimPosition));
        GameObject projectileObject = Instantiate(projectilePrefab, summonPoint.position, projectileAngle);
        BossProjectile projectileScript = projectileObject.GetComponent<BossProjectile>();
        Vector3 projectileMotion = relativeAimPosition.normalized*projectileSpeed;
        projectileScript.bossProjectileVelocity = projectileMotion;
    }
}
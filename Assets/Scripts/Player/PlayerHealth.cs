using TMPro;
using UnityEngine.UI;
using UnityEngine;

// Manages the players health and the UI element for it
public class PlayerHealth : MonoBehaviour {
    // Keeps track of the health
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 100f;

    // UI elements that get updated based on health
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;

    // External reference to reset game if the player is defeated
    private BossMLAgent bossAI;

    // Resets values to default on startup and loads in AI and existence checks if it is loaded
    // Inputs: None
    // Outputs: None
    private void Start() {
        currentHealth = maxHealth;
        if(BossMLAgent.bossAI!=null){
            bossAI = BossMLAgent.bossAI;
        }
        else{
            Debug.LogError("ERROR! Boss not loaded");
        }
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    // Damages the player based on a random amount
    // Input: Amount of damage from a random number generator in BossProjectile.cs
    // Output: None
    public void DamagePlayer(int amount){
        currentHealth-=amount;
        // Range check to see if the damage has passed a tipping point
        if(currentHealth<=0f){
            bossAI.EndEpisode();
        }
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        // Range check to ensure that the values are within the maximum and minimum range
        healthBar.value = Mathf.Clamp(currentHealth/maxHealth,healthBar.minValue,healthBar.maxValue);
    }

    // Called to reset all values related to player health are reset properly
    // Input: None
    // Output: None
    public void ResetHealth(){
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        // Range check that sets health bar to max or min value if outside the range
        healthBar.value = Mathf.Clamp(currentHealth/maxHealth,healthBar.minValue,healthBar.maxValue);
    }
}
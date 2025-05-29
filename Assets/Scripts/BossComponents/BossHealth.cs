using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Handles the bosses health UI and logic for when defeated
public class BossHealth : MonoBehaviour {
    // External reference to boss
    private BossMLAgent bossAI;
    // Health settings
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    // UI Management
    [SerializeField] private Slider leftBossBarSide;
    [SerializeField] private Slider rightBossBarSide;
    [SerializeField] private TMP_Text healthText;
    // Game loop and logic management
    [SerializeField] private GameObject gameManager;

    // Initializes at the beginning of script life time
    // Input: None
    // Output: None
    private void Start() {
        // Existence check for the MLAgent script
        if(BossMLAgent.bossAI != null){
            bossAI = BossMLAgent.bossAI;
        }
        else{
            Debug.LogError("ERROR! Could not load boss AI");
        }
        // Ensures no errors occur when loading health by resetting it to max
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }
    // Damages the boss a certain amount that is precalculated by the projectile that hit the boss
    // Input: The amount of damage the boss should take
    // Output: None
    public void DamageBoss(int amount){
        currentHealth -= amount;
        // Range check to win the game if all the health is depleted
        if(currentHealth <= 0){
            gameManager.GetComponent<GameManager>().GameWin();
        }
        // Range check to ensure the health bar does not go into the negatives
        leftBossBarSide.value = Mathf.Clamp(currentHealth/maxHealth,leftBossBarSide.minValue,leftBossBarSide.maxValue);
        rightBossBarSide.value = Mathf.Clamp(currentHealth/maxHealth,rightBossBarSide.minValue,rightBossBarSide.maxValue);

        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        
    }

    // Resets the boss health when called
    // Input 
    // Output: None
    public void ResetHealth(){
        currentHealth = maxHealth;
        // Range checks the health bar to ensure it is set to maximum
        leftBossBarSide.value = Mathf.Clamp(currentHealth/maxHealth,leftBossBarSide.minValue,leftBossBarSide.maxValue);
        rightBossBarSide.value = Mathf.Clamp(currentHealth/maxHealth,rightBossBarSide.minValue,rightBossBarSide.maxValue);
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }
}
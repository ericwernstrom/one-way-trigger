using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int currentXP = 0;
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private int xpToNextLevel = 100;
    private int levelProgressXP = 0; // XP earned since last level-up
    private int maxXP = 100;

    //Explosion upgrade variables
    [SerializeField]
    private static int explosionMaxLevel = 5;
    [SerializeField]
    private static int explosionCurrentLevel = 0; // Current level for explosion upgrades

    //HUD components
    [SerializeField]
    private Slider experienceBar;
    [SerializeField]
    TextMeshProUGUI XPAmountText;
    [SerializeField]
    TextMeshProUGUI levelText;

    // AUDIO
    [SerializeField]
    private AudioClip levelUpSound;
    [SerializeField]
    private AudioMixerGroup mixerGroup;

    public static void ExplosionScaleUpgrade() 
    { 
        explosionCurrentLevel++;
        if (explosionCurrentLevel < explosionMaxLevel)
        {
            Rocket_script.explosionScale += 0.5f;
            Grenade.explosionScale += 0.5f;
            StickyBomb.explosionScale += 0.5f;
            Weapon_Explosion.max_scale += 0.5f;
        }
    }

    public static void ResetUpgrades() 
    { 
        explosionCurrentLevel = 0;
        Rocket_script.explosionScale = 2f;
        Grenade.explosionScale = 2f;
        StickyBomb.explosionScale = 2f;
        Weapon_Explosion.max_scale = 3f;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        levelProgressXP += amount;

        // Check for level up
        while (levelProgressXP >= xpToNextLevel)
        {
            levelProgressXP -= xpToNextLevel;
            level++;

            // AUDIO: Play level up sound
            AudioUtils.PlayClipAtPointToMixer(levelUpSound, transform.position, mixerGroup);

            // Increase XP requirement for next level (scaling)
            xpToNextLevel += 50;

            // Track the total XP needed to reach this level
            maxXP += xpToNextLevel;

            levelText.text = level.ToString();

            if (GameManagerScript.Instance != null)
            {
                GameManagerScript.Instance.showLevelUpScreen();
            }
            else
            {
                Debug.LogWarning("GameManagerScript.Instance is null!");
            }
        }

        experienceBar.maxValue = xpToNextLevel;
        experienceBar.value = levelProgressXP;
        XPAmountText.text = currentXP.ToString() + " exp / " + maxXP + " exp";
    }
}


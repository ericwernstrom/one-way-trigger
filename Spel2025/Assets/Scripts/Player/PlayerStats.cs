using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    //HUD components
    [SerializeField]
    private Slider experienceBar;
    [SerializeField]
    TextMeshProUGUI XPAmountText;
    [SerializeField]
    TextMeshProUGUI levelText;


    public void AddXP(int amount)
    {
        currentXP += amount;
        levelProgressXP += amount;

        // Check for level up
        while (levelProgressXP >= xpToNextLevel)
        {
            levelProgressXP -= xpToNextLevel;
            level++;

            // Increase XP requirement for next level (scaling)
            xpToNextLevel += 50;

            // Track the total XP needed to reach this level
            maxXP += xpToNextLevel;

            levelText.text = level.ToString();
        }

        experienceBar.maxValue = xpToNextLevel;
        experienceBar.value = levelProgressXP;

        XPAmountText.text = currentXP.ToString() + " exp / " + maxXP + " exp";
    }
}


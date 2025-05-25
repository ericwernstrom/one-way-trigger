using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LevelUpSelector;
using TMPro;

public class LevelUpSelector : MonoBehaviour
{
    [System.Serializable]
    public struct Upgrade
    {
        //Action stores a void function with no parameters
        public Action action;
        public Func<int> getLevel;
        //Maybe add maxlevel here if different upgrades have different max levels. Right now all have 5
        public string description;
        public Sprite icon;

        //hard coded max level
        public bool IsMaxed() => getLevel?.Invoke() >= 5;
    }

    private PlayerHealth playerHealth;
    private ShootProjectile shootProjectile;
    private PlayerLocomotion playerMoveSpeed;

    // This list will store the upgrade actions
    private List<Upgrade> upgrades = new List<Upgrade>();
    //Available upgrades
    private List<int> availableUpgradeIndices = new List<int>();


    // Reference to your UI buttons (assign in inspector)
    [SerializeField] private Button[] upgradeButtons;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMoveSpeed = player.GetComponent<PlayerLocomotion>();

        // Prepare upgrades with all three components
        //Health Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => playerHealth.IncreaseMaxHealth(),
            getLevel = () => playerHealth.CurrentHealthLevel(),
            description = "MAX HEALTH",
            icon = Resources.Load<Sprite>("Sprites/Health_up") // Load from Resources folder
        });

        //Movement speed Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => playerMoveSpeed.SpeedUpgrade(),
            getLevel = () => playerMoveSpeed.GetCurrentSpeedLevel(),
            description = "MOVE SPEED",
            icon = Resources.Load<Sprite>("Sprites/Rocket_boots")
        });

        //Gravity Bullet Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => GravityBullet_script.DamageUpgrade(),
            getLevel = () => GravityBullet_script.GetCurrentGravityLevel(),
            description = "GRAVITY GUN DAMAGE",
            icon = Resources.Load<Sprite>("Sprites/Gravity_upgrade")
        });

        
        //Rocket Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => Weapons_Explosion_script.ExplosionLevelUp(),
            getLevel = () => Weapons_Explosion_script.GetCurrentExplosionDamageLevel(),
            description = "EXPLOSIVE DAMAGE",
            icon = Resources.Load<Sprite>("Sprites/Explosion_damage")
        });

        //Explosion Scale Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => PlayerStats.ExplosionScaleUpgrade(),
            getLevel = () => PlayerStats.GetExplosionCurrentLevel(),
            description = "EXPLOSION SCALE",
            icon = Resources.Load<Sprite>("Sprites/Explosive_upgrade")
        });
        
        RefreshAvailableUpgrades();
        RandomizeUpgradeList();
    }

    private void RefreshAvailableUpgrades()
    {
        availableUpgradeIndices.Clear();
        for (int i = 0; i < upgrades.Count; i++)
        {
            if (!upgrades[i].IsMaxed())
            {
                availableUpgradeIndices.Add(i);
            }
        }
    }

    private void RandomizeUpgradeList()
    {
        RefreshAvailableUpgrades();

        // Check if all upgrades are maxed
        if (availableUpgradeIndices.Count == 0)
        {
            Debug.Log("All upgrades are maxed!");

            foreach (var button in upgradeButtons)
            {
                // Set label to indicate no upgrades
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                Image buttonIcon = button.GetComponentInChildren<Image>();

                buttonText.text = "ALL UPGRADES MAXED";
                buttonIcon.sprite = Resources.Load<Sprite>("Sprites/Crit_up"); // Optional: clear icon or set a default "maxed" sprite
                button.gameObject.SetActive(true); // Make sure it's visible
            }

            return;
        }

        // Fisher-Yates Shuffle
        for (int i = availableUpgradeIndices.Count - 1; i > 0; i--)
        {
            int randIndex = UnityEngine.Random.Range(0, i + 1);
            var temp = availableUpgradeIndices[i];
            availableUpgradeIndices[i] = availableUpgradeIndices[randIndex];
            availableUpgradeIndices[randIndex] = temp;
        }

        // Update each button
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < availableUpgradeIndices.Count)
            {
                int upgradeIndex = availableUpgradeIndices[i];
                var upgrade = upgrades[upgradeIndex];

                // Get the button child Text and Image components
                TextMeshProUGUI buttonText = upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                Image buttonIcon = upgradeButtons[i].GetComponentInChildren<Image>();

                // Update them
                buttonText.text = $"{upgrade.description} {upgrade.getLevel?.Invoke() ?? 0}/5";
                buttonIcon.sprite = upgrade.icon;
            }
            else
            {
                // Hide unused buttons if fewer upgrades than buttons
                upgradeButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Button click handlers
    public void Button1() => UseUpgrade(0);
    public void Button2() => UseUpgrade(1);
    public void Button3() => UseUpgrade(2);
    //public void Button4() => ExecuteUpgrade(3); // If you add a 4th button

    private void UseUpgrade(int index)
    {
        if (index < availableUpgradeIndices.Count)
        {
            int upgradeIndex = availableUpgradeIndices[index];
            //Use upgrade if there is an action defined, if all are maxed there will not be any actions defined
            upgrades[upgradeIndex].action?.Invoke();

            RefreshAvailableUpgrades();
            RandomizeUpgradeList();
        }
    }

}

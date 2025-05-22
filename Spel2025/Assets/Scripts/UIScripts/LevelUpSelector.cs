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
        public string description;
        public Sprite icon;
    }

    private PlayerHealth playerHealth;
    private ShootProjectile shootProjectile;
    private PlayerLocomotion playerMoveSpeed;

    // This list will store the shuffled upgrade actions
    private List<Upgrade> upgrades = new List<Upgrade>();
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
            description = "MAX HEALTH +",
            icon = Resources.Load<Sprite>("Sprites/Health_up") // Load from Resources folder
        });

        //Movement speed Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => playerMoveSpeed.SpeedUpgrade(),
            description = "MOVE SPEED +",
            icon = Resources.Load<Sprite>("Sprites/Rocket_boots")
        });

        //Gravity Bullet Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => GravityBullet_script.DamageUpgrade(),
            description = "GRAVITY GUN DAMAGE +",
            icon = Resources.Load<Sprite>("Sprites/Gravity_upgrade")
        });

        //Rocket Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => Weapons_Explosion_script.ExplosionLevelUp(),
            description = "EXPLOSIVE DAMAGE +",
            icon = Resources.Load<Sprite>("Sprites/Explosion_damage")
        });

        //Explosion Scale Upgrade
        upgrades.Add(new Upgrade
        {
            action = () => PlayerStats.ExplosionScaleUpgrade(),
            description = "EXPLOSION SCALE +",
            icon = Resources.Load<Sprite>("Sprites/Explosive_upgrade")
        });

        RandomizeUpgradeList();
    }

    private void RandomizeUpgradeList()
    {
        // Fisher-Yates Shuffle
        for (int i = upgrades.Count - 1; i > 0; i--)
        {
            int randIndex = UnityEngine.Random.Range(0, i + 1);
            var temp = upgrades[i];
            upgrades[i] = upgrades[randIndex];
            upgrades[randIndex] = temp;
        }

        // Update each button
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < upgrades.Count)
            {
                // Get the button child Text and Image components
                TextMeshProUGUI buttonText = upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                Image buttonIcon = upgradeButtons[i].GetComponentInChildren<Image>();

                // Update them
                buttonText.text = upgrades[i].description;
                buttonIcon.sprite = upgrades[i].icon;
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
        if (index < upgrades.Count)
        {
            upgrades[index].action?.Invoke();
            RandomizeUpgradeList();
        }
    }

}

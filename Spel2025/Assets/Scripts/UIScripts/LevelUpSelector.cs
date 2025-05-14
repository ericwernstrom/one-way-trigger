using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSelector : MonoBehaviour
{

    private PlayerHealth playerHealth;
    private ShootProjectile shootProjectile;
    private PlayerLocomotion playerMoveSpeed;

    // This list will store the shuffled upgrade actions
    private List<Action> upgradeActions = new List<Action>();

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMoveSpeed = player.GetComponent<PlayerLocomotion>();

        // Prepare upgrade actions
        upgradeActions.Add(() => playerHealth.IncreaseMaxHealth());
        upgradeActions.Add(() => playerMoveSpeed.SpeedUpgrade());
        upgradeActions.Add(() => GravityBullet_script.DamageUpgrade());
        // You can add more here later, like:
        // upgradeActions.Add(() => shootProjectile.FireRateUpgrade());

        //RandomizeUpgradeList();
    }
    /*
    private void RandomizeUpgradeList()
    {
        // Fisher-Yates Shuffle
        for (int i = upgradeActions.Count - 1; i > 0; i--)
        {
            int randIndex = UnityEngine.Random.Range(0, i + 1);
            var temp = upgradeActions[i];
            upgradeActions[i] = upgradeActions[randIndex];
            upgradeActions[randIndex] = temp;
        }
    }

    // Button click handlers
    public void Button1() => UseUpgrade(0);
    public void Button2() => UseUpgrade(1);
    public void Button3() => UseUpgrade(2);
    //public void Button4() => ExecuteUpgrade(3); // If you add a 4th button

    private void UseUpgrade(int index)
    {
        if (index < upgradeActions.Count)
        {
            upgradeActions[index]?.Invoke();
            RandomizeUpgradeList();
        }
    }
    */

    /* 
    
    Kanske byta ut nedanstående funktioner till en per button sen spara alla upgrades i en lista så att
    Lista innehåller exempelvis. hpUpgrade =  playerHealth.IncreaseMaxHealth(); osv.
    Sen randomize och 
    
    
    */
    public void Button1()
    {
        playerHealth.IncreaseMaxHealth();
    }

    public void Button2() 
    { 
        playerMoveSpeed.SpeedUpgrade();
    }
    public void Button3()
    {
        GravityBullet_script.DamageUpgrade();
    }

}

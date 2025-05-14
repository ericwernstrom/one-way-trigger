using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSelector : MonoBehaviour
{

    private PlayerHealth playerHealth;
    private ShootProjectile shootProjectile;
    private PlayerLocomotion playerMoveSpeed;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMoveSpeed = player.GetComponent<PlayerLocomotion>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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


    /*
    public void UpgradeFireRate()
    {
        ShootProjectile.FireRateUpgrade();
        //GameManagerScript.Instance.chooseBuff();
    }
    */

}

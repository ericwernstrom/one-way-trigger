using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField]
    private int selectedWeapon = 0; // The weapon that is selected
    void Start()
    {
        SelectWeapon();
    }

    void Update(){
        int previousSelectedWeapon = selectedWeapon;
        // Select weapon with 1 and 2 (1 = Assault Rifle, 2 = Shotgun) Idk how to make it not hardcoded right now
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    // Checks which weapon is selcted and sets the weapon active, the rest are inactive
    void SelectWeapon() {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}

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
    private Slider experienceBar;
    [SerializeField]
    TextMeshProUGUI XPAmount;


    public void AddXP(int amount)
    {
        currentXP += amount;
        experienceBar.value = currentXP;
        //Hardcoded "out of 100" exp. Change here for leveling up.
        XPAmount.text = currentXP.ToString() + " exp / 100 exp";
        Debug.Log("XP added: " + currentXP);
    }
}


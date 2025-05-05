using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int currentXP = 0;
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private Slider experienceBar;

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP added: " + currentXP);
    }
}


using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentXP = 0;
    public int level = 1;

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP added: " + currentXP);
    }
}


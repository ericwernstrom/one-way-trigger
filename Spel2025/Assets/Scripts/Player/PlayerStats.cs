using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int currentXP = 0;
    [SerializeField]
    private int level = 1;

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP added: " + currentXP);
    }
}


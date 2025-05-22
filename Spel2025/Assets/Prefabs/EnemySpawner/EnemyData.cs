using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public GameObject enemyPrefab;
    public float baseWeight; // Starting weight
    public float weightIncreaseRate; // How much the weight increases per difficulty step
}
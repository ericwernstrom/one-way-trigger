using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    public Transform player; // Reference to the player's transform

    void Update()
    {
        // Ensure the enemy only rotates around the Y-axis
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // This makes sure the enemy only rotates around the Y-axis

        // Rotate the enemy to face the player
        if (direction != Vector3.zero) // Ensure the direction is not zero to avoid any errors
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}

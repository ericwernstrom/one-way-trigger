using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody enemyRb;
    [SerializeField]
    private float moveSpeed = 5f; // Speed of movement
    [SerializeField]
    private float jumpForce = 5f; // Force applied for jumping

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // Get input from keyboard
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Apply the movement to the Rigidbody
        enemyRb.AddForce(movement * moveSpeed);

        // Optional: Handle jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        // Apply an upward force to the Rigidbody for jumping
        enemyRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}

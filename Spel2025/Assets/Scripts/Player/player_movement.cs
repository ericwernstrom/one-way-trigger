using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement Options")]
    [SerializeField]
    private float Speed;
    [SerializeField]
    private float jumpForce;
    float Horizontal;
    float Vertical;
    float JumpInput;

    [Header("Ground Check")]
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundDrag;
    [SerializeField]
    private float airDrag;
    [SerializeField]
    private float playerHeight;
    bool grounded;

    [SerializeField]
    private Transform orientation;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Function for registering Inputs
    private void PlayerInput()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        JumpInput = Input.GetAxis("Jump");
    }

    // Function for moving the player
    private void MovePlayer()
    {
        direction = orientation.forward * Vertical + orientation.right * Horizontal;

        if (grounded)
        {
            rb.AddForce(direction.normalized * Speed * 10f, ForceMode.Force);
        } else
        {
            rb.AddForce(direction.normalized * Speed * 4f, ForceMode.Force);
        }

    }

    // Function for making the player jump
    private void Jump() { 
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundLayer);

        // Checks if the player input anything
        PlayerInput();

        // Handles the friction
        if (grounded) { Debug.Log("grounded");
            rb.drag = groundDrag;
        } else { rb.drag = airDrag; }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if(JumpInput == 1 && grounded)
        {
            Jump();
        }
    }
}
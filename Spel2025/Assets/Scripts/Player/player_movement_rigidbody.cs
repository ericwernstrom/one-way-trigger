using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class player_movement_rigidbody : MonoBehaviour
{

    [SerializeField]
    private float Speed;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float jump_power;
    [SerializeField]
    private float jump_power_held;
    [SerializeField]
    private float coyote_time;
    [SerializeField]
    private float jump_input_buffer;
    [SerializeField]
    private float player_height;
    [SerializeField]
    private float friction;
    [SerializeField]
    private Transform Cam;
    private Rigidbody rb;

    Vector3 Movement = Vector3.zero;
    private bool cant_jump = false;
    private bool jump_held;
    private float y_multiplier;
    private float current_coyote_time;
    private float current_jump_input_buffer;
    private float Jump_input;
    private float ray_distance = 0.1f;
    private bool IsGrounded;
    private bool Jump_buffer_available;

    Ray ray_down = new Ray();
    RaycastHit hit_down;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Jump_buffer_available = true;
        current_jump_input_buffer = jump_input_buffer;
    }

    private void Jump()
    {
        jump_held = true;
        rb.AddForce(Vector3.up * jump_power);
    }


    private void In_Air()
    {
        // in air decrease y velocity with gravity, less if jump is held
        current_jump_input_buffer += Time.deltaTime;


        if (Jump_input == 1)
        {
            if (!cant_jump && jump_held && rb.velocity.y > 0)
            {
                y_multiplier = jump_power_held;
            }
            else if (!jump_held)
            {
                current_jump_input_buffer = 0f;
                Jump_buffer_available = true;
                jump_held = true;
            }
        }
        // reset jump if jump button is let go
        else
        {
            y_multiplier = 1;
            jump_held = false;
            cant_jump = true;
        }
    }

    private void Jump_check()
    {
        // region for jump


        // reset jump when button is let go
        // when the player is grounded jump is possible if jump button is not held since last jump
        if (IsGrounded)
        {

            y_multiplier = 1;

            current_coyote_time = 0;

            if (cant_jump)
            {
                cant_jump = false;
            }
            if (Jump_input == 0)
            {
                jump_held = false;
            }

            if (!jump_held)
            {
                if (Jump_input == 1)
                {
                    Jump();
                }
            }
            else if (current_jump_input_buffer < jump_input_buffer && Jump_buffer_available)
            {
                Jump_buffer_available = false;
                Jump();
            }
        }

        
        else 
        {
            // jump held time, how long the jumo button can be hald in air and extend jump
            current_coyote_time += Time.deltaTime;
            In_Air();

            if ((current_coyote_time < coyote_time && Jump_input == 1) && !jump_held)
            {
                Jump();
            }
        }
    }


    private bool GroundCheck()
    {
        // l�gg till collision ground check
        ray_down.origin = transform.position + new Vector3(0, -player_height, 0);
        ray_down.direction = new Vector3(0, -1f, 0);
        if (Physics.Raycast(ray_down, out hit_down, ray_distance))
        {
            return true;
        }
        else return false;
    }
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layer
        IsGrounded = false;
        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 point = other.GetContact(i).point;
            if (point.y < transform.position.y)
            {
                IsGrounded = true;
                break;
            }
        }    
    }
    private void OnCollisionExit(Collision other)
    {
        IsGrounded = false;
    }


    // Update is called once per frame

    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        Jump_input = Input.GetAxis("Jump");

        Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;

        //IsGrounded = GroundCheck();
        Jump_check();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            rb.AddForce(Movement.normalized * 20);

        if (IsGrounded)
        {
            rb.AddForce(Movement.normalized * Speed * 10f);
        }
        else
        {
            rb.AddForce(Movement.normalized * Speed * 10f);           
            rb.AddForce(Vector3.down * Time.deltaTime * gravity * y_multiplier);

        }

        rb.AddForce(-rb.velocity.x * friction, 0, -rb.velocity.z * friction);



    }

    // Freezes the rotation in the x and z axis
    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

}
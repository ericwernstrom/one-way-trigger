using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove_with_playercontroller : MonoBehaviour
{


    CharacterController Controller;

    public float Speed;
    public float gravity;
    public float jump_power;
    public float jump_power_held;
    public float jump_max_time;
    public float coyote_time;
    public float jump_input_buffer;
    public Transform Cam;

    Vector3 Movement = Vector3.zero;
    private bool cant_jump = false;
    private float current_jump_time;
    private bool jump_held;
    private float current_y_velocity;
    private float current_coyote_time;
    private float current_jump_input_buffer;
    private float Jump_input;

    // Start is called before the first frame update
    void Start()
    {
        current_jump_input_buffer = jump_input_buffer;
        Controller = GetComponent<CharacterController>();

    }

    private void On_Ground()
    {
            if (cant_jump)
            {
                cant_jump = false;
            }

            

            if (Jump_input == 1 || current_jump_input_buffer < jump_input_buffer)
            {
            //inital velocity of jump
            if(!jump_held)
            {
                jump_held = true;
                current_jump_time = 0;
                current_y_velocity = jump_power;
            }
            else if(current_jump_input_buffer < jump_input_buffer)
            {
                jump_held = true;
                current_jump_time = 0;
                current_y_velocity = jump_power;
            }
            }
    }
    
    private void In_Air()
    {
        // in air decrease y velocity with gravity, less if jump is held
            current_y_velocity -= gravity * Time.deltaTime;
            current_jump_input_buffer += Time.deltaTime;
            if(Jump_input == 1)
            {
                if (!cant_jump && jump_held)
                {
                    current_y_velocity += jump_power_held * Time.deltaTime;
                    current_jump_time += Time.deltaTime;
                }
                else if(!jump_held)
                {
                    current_jump_input_buffer = 0f; 
                    jump_held = true;
                }
            }
            // reset jump if jump button is let go
            else
            {
                cant_jump = true;
            }
    }

    private void Jump(){
                // region for jump


        // reset jump when button is let go
        if (Jump_input == 0)
        {
            jump_held = false;
        }

        // when the player is grounded jump is possible if jump button is not held since last jump
        if (Controller.isGrounded)
        {
            if(coyote_time != 0)
            {
                current_coyote_time = 0;
            }
            On_Ground();
        }

        if (!Controller.isGrounded)
        {
        // jump held time, how long the jumo button can be hald in air and extend jump

        if(current_jump_time > jump_max_time)
        {
            cant_jump = true;
        }
            current_coyote_time += Time.deltaTime;
            In_Air();
            if(current_coyote_time < coyote_time && Jump_input == 1)
            {
                On_Ground();
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        Jump_input = Input.GetAxis("Jump");
        float MovementY = Movement.y;
        Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        //Movement.y = 0f;



        Jump();

        // add y velocity to movement
        Movement.y = current_y_velocity;
        Controller.Move(Movement);
    }

}
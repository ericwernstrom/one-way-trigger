using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    CharacterController Controller;

    [SerializeField]
    private float Speed;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float jump_power;

    [SerializeField]
    private Transform Cam;
    Vector3 Movement = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
    Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        float Jump = Input.GetAxis("Jump");
        float MovementY = Movement.y;
        Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        //Movement.y = 0f;



        // region for jump
        if (Controller.isGrounded && Jump == 1)
        {
            Movement.y = jump_power;
        }
        else
        {
            Movement.y = MovementY;
        }
        if (!Controller.isGrounded)
        {
            Movement.y -= gravity * Time.deltaTime;
        }

        Controller.Move(Movement);


    }
}
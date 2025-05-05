using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;

    Vector3 moveDirection;
    Rigidbody playerRigidbody;

    [SerializeField]
    private float movementSpeed = 7f;
    [SerializeField]
    private float rotationSpeed = 15f;
    [SerializeField]
    private Transform cameraObject;
    [SerializeField]
    private float groundFriction = 1f;
    [SerializeField]
    private float airFriction = 1f;

    [Header("Falling")]
    //private float inAirTimer;
    [SerializeField]
    private float fallingVelocity;
    [SerializeField]
    private float leapingVelocity;
    [SerializeField]
    private float jumpVelocity = 100f;
    [SerializeField]
    private LayerMask groundLayer;
    private float rayCastHeightOffset = 0.5f;

    [Header("Movement Flags")]
    [SerializeField]
    private bool isGrounded;
    //private bool isJumping;

    private bool rotationLocked = false;
    private float rotationLockTimer = 0f;


    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
  
        //if (playerManager.inInteracting) return;
        HandleMovement();

        //For smoother rotation after shooting
        if (rotationLocked)
        {
            rotationLockTimer -= Time.deltaTime;
            if (rotationLockTimer <= 0f)
            {
                rotationLocked = false;
            }
        }

        if (!rotationLocked)
        {
            HandleRotation(); // only allow rotation based on movement when not locked
        }

    }

    //For smoother rotation
    public void TemporarilyDisableMovementRotation(float duration = 0.5f)
    {
        rotationLocked = true;
        rotationLockTimer = duration;
    }


    private void HandleMovement()
    {
        /*
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
        */

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;

        playerRigidbody.AddForce(movementVelocity);
        if (isGrounded)
        {
            playerRigidbody.AddForce(-playerRigidbody.velocity.x * groundFriction, 0, -playerRigidbody.velocity.z * groundFriction);
        }
        else {
            playerRigidbody.AddForce(-playerRigidbody.velocity.x * airFriction, -playerRigidbody.velocity.y * airFriction, -playerRigidbody.velocity.z * airFriction);
        }
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection.magnitude == 0)
            return; // No input, keep current rotation

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        float maxDistance = 1.5f;
        
        if (!isGrounded)
        {
            //inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity);
            //  * inAirTimer
        }
        if (Physics.SphereCast(rayCastOrigin, 0.3f, -Vector3.up, out hit, maxDistance, groundLayer))
        {
            //if (!isGrounded) // && !playerManager.isInteracting
            //{
            //animatorManager.PlayTargetAnimation("Land", true);
            //}
            //inAirTimer = 0;
            isGrounded = true; 
        }
        else
        {
            isGrounded = false;
        }
    }
    public void HandleJumping()
    {
        //jump animation
        if (isGrounded)
        {
            playerRigidbody.AddForce(0, jumpVelocity, 0);
        }
    }
}

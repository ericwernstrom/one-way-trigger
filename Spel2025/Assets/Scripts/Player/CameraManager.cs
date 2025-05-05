using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    [SerializeField]
    private Transform targetTransform; // the object the camera will follow
    [SerializeField]
    private Transform cameraPivot; // the object the camera uses to pivot
    [SerializeField]
    private Transform cameraTransform; // transform of the actual camera
    [SerializeField]
    private LayerMask collisionLayers; // The layers the camera will collide with
    private float defaultPosition;
    private Vector3 cameraVectorPosition;
    private Vector3 cameraFollowVelocity = Vector2.zero;

    [SerializeField]
    private float cameraFollowSmoothing = 2f;
    [SerializeField]
    private float cameraLookSpeed = 2f;
    [SerializeField]
    private float cameraPivotSpeed = 2f;
    [SerializeField]
    private float minimumPivotAngle = -60f;
    [SerializeField]
    private float maximumPivotAngle = 60f; 
    [SerializeField]
    private float cameraCollisionRadius = 0.2f; // size of the camera collision sphere
    [SerializeField]
    private float cameraCollisionOffset = 0.2f; // how much the camera will bounce away from ojects
    [SerializeField]
    private float minimumCollisionOffset= 0.2f;


    public float lookAngle; // Camera look up and down
    public float pivotAngle; // camera left and right


    private void Awake()
    {
        inputManager = Object.FindFirstObjectByType<InputManager>();
        //targetTransform = Object.FindFirstObjectByType<PlayerManager>().transform;

        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSmoothing);
        transform.position = targetPosition;


    }

    private void RotateCamera()
    {
        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;

    }

}

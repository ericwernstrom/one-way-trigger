using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    private const float YMin = -50.0f;
    private const float YMax = 50.0f;

    [SerializeField]
    private Transform lookAt;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    [SerializeField]
    private float sensivity = 4.0f;

    // zoom settings
    float current_fieldOfView;
    [SerializeField]
    private float min_fieldOfView;
    [SerializeField]
    private float max_fieldOfView;
    float current_cameraOffset = 0.0f;
    [SerializeField]
    private float max_cameraOffset = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        // removes mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        current_fieldOfView = max_fieldOfView;
    }

    // Update is called once per frame
    void LateUpdate()
    {


        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime * -1f;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0.0f, 0.0f, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0.0f);
        transform.position = lookAt.position + rotation * Direction;

        transform.LookAt(lookAt.position);
          // set camera field of view


        float zoom = Input.GetAxis("Fire2");
        Camera.main.fieldOfView = current_fieldOfView;
        // when fire 2 is held, zoom in by changing the field of view until it reaches minimum value, reverse for zoom out
        if(zoom == 1 && current_fieldOfView > min_fieldOfView) {
            current_fieldOfView += -150.0f * Time.deltaTime;
        } else if(zoom == 0 && current_fieldOfView < max_fieldOfView) {
            current_fieldOfView += 150.0f * Time.deltaTime;
            current_cameraOffset += -20.0f * Time.deltaTime;      
        } else {
        // safe state if the camera messes up
            if(current_fieldOfView < min_fieldOfView)
            {
                current_fieldOfView = min_fieldOfView;
            }
            if (current_fieldOfView > max_fieldOfView)
            {
                current_fieldOfView = max_fieldOfView;
            }
        }



        // handles the camera offset when fire 2 is held
        if (zoom == 1 && current_cameraOffset < max_cameraOffset)
        {
            current_cameraOffset += 20.0f * Time.deltaTime;
        }
        else if (zoom == 0 && current_cameraOffset > 0)
        {
            current_cameraOffset += -20.0f * Time.deltaTime;
        }
    }
}


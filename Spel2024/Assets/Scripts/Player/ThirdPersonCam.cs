using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public float rotationSpeed;
    public bool isInCombat = false;
    public bool isAiming = false;
    public CinemachineFreeLook NormalCam;
    
    public float zoomedIn = 80f;
    public float zoomedOut = 100f;
    
    // public CinemachineFreeLook CombatCam;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        // To later switch between the cameras
        //NormalCam.enabled = true;
        //CombatCam.enabled = true;

        //NormalCam.Priority = 10;
        //CombatCam.Priority = 0;
    }

    void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * Vertical + orientation.right * Horizontal;

        // Needs smoothing
        if (isInCombat){ 
            playerObj.forward = orientation.forward;
        }
        // Needs smoothing
        if (isAiming){
            NormalCam.m_Lens.FieldOfView = zoomedIn;
        }
        else {
            NormalCam.m_Lens.FieldOfView = zoomedOut;
        }

        if(inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        /*
        if (Input.GetKeyDown(KeyCode.H))
        {
            switchCameraStyle();
        }
        */
    }
    /*
    private void switchCameraStyle(){
        // Switch between cameras depending on which one is currently active
        if (NormalCam.Priority > CombatCam.Priority)
        {
            NormalCam.Priority = 0;
            CombatCam.Priority = 10;
        }
        else
        {
            NormalCam.Priority = 10;
            CombatCam.Priority = 0;
        }
    }
    */
}


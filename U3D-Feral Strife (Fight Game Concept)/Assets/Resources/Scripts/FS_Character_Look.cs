//=========== Written by Arthur W. Sheldon AKA Lizband_UCC ====================
//
// SID: MRC
// Purpose: Controll a characters camera and look direction
// Applied to: The root of network or local player character 
// Editor script: 
// Notes: 
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_Character_Look : MonoBehaviour
{
    // Public variables
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;
    public float positiveRotationLimit = 70f;
    public float negativeRotationLimit = -90f;

    // Private variables
    private float lookX;
    private float lookY;
    private float lookMultiplier = 0.1f;

    private float xRotation;
    private float yRotation;

    // Reference variables
    private Camera playerCamera;
    private Transform cameraPivot;


    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        cameraPivot = playerCamera.transform.parent.gameObject.transform;

        // Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        PlayerInput();

        // Rotate the player character while looking around
        cameraPivot.localRotation = Quaternion.Euler(xRotation,0,0);
        transform.rotation = Quaternion.Euler(0,yRotation,0);
    }


    public void PlayerInput()
    {
        lookX = Input.GetAxisRaw("Mouse X"); // Remove me when using the NUSDK
        lookY = Input.GetAxisRaw("Mouse Y"); // Remove me when using the NUSDK

        yRotation += lookX * sensitivityX * lookMultiplier;
        xRotation -= lookY * sensitivityY * lookMultiplier;

        // Clamp vertical camera rotation
        xRotation = Mathf.Clamp(xRotation, negativeRotationLimit, positiveRotationLimit);
    }
}

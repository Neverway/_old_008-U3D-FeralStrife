//=========== Written by Arthur W. Sheldon AKA Lizband_UCC ====================
//
// SID: MRC
// Purpose: Controll a characters movements and stats
// Applied to: The root of network or local player character 
// Editor script: 
// Notes: 
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_Character_Controller : MonoBehaviour
{
    // Public variables
    [Header ("Movement")]
    [Range (0, 2)]
    public int playerID;
    public float moveSpeed = 6f;
    public float groundMovementMultiplier = 10f;
    public float airMovementMultiplier = 0.4f;

    [Header ("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 2f;

    [Header ("Sprinting")]
    public float walkSpeed = 4f;
    public float runSpeed = 6f;
    public float accelerationSpeed = 10f;

    [Header ("Jumping")]
    public bool isGrounded;
    public float playerHeight = 1f;
    public float jumpForce = 15f;
    public float groundDistance = 0.4f;
    [SerializeField] Transform groundCheckLfoot;
    [SerializeField] Transform groundCheckRfoot;

    [Header ("Health 'n Stats")]
    [Range (0, 999)]
    public float damage = 0f;
    public string entityName = "Proto-Buddy";
    public float weight = 100;
    

    [Header ("IK Rig")]
    public bool useIK;
    public float maxLegDistance;
    public LayerMask layerMask;
    public Vector3 stepOffset;

    [Header ("References")]
    public Transform leftLegTarget;
    public Transform rightLegTarget;

    public Transform leftLegIdeal;
    public Transform rightLegIdeal;

    public Transform leftRaycast;
    public Transform rightRaycast;

    // Private variables

    // Movement
    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 moveDirection;

    // IK
    private Vector3 initLeftLegPos;
    private Vector3 initRightLegPos;
    private Vector3 lastLeftLegPos;
    private Vector3 lastRightLegPos;

    // Ground detection
    private RaycastHit hit;
    private RaycastHit hitSlope;
    private Vector3 slopedMoveDirection;

    // Reference variables
    public Rigidbody playerRigidbody;
    private Animator playerAnimator;


    private void Start()
    {
        // Find references
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        // Set intial values
        if (useIK)
        {
            initLeftLegPos = leftLegTarget.localPosition;
            initRightLegPos = rightLegTarget.localPosition;

            lastLeftLegPos = leftLegTarget.position;
            lastRightLegPos = rightLegTarget.position;
        }
    }


    private void Update()
    {
        //if (Physics.CheckSphere(transform .position - new Vector3(0, playerHeight / 2 + 0.15f, 0), groundDistance, layerMask))
        if (Physics.CheckSphere(groundCheckLfoot.position, groundDistance, layerMask) || Physics.CheckSphere(groundCheckRfoot.position, groundDistance, layerMask) )
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (playerID == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // Remove me when using the NUSDK
            {
                Jumping();
            }

            PlayerInput();
        }
        Drag();
        Speed();
        IKRigging();
        slopedMoveDirection = Vector3.ProjectOnPlane(moveDirection, hitSlope.normal);
    }    
    

    private void FixedUpdate()
    {
        Movement();
    }


    private void PlayerInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal"); // Remove me when using the NUSDK
        verticalMovement = Input.GetAxisRaw("Vertical"); // Remove me when using the NUSDK

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimator.Play("Jab_0");
        }
    }


    private void Jumping()
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    private void Movement()
    {
        if (isGrounded && !onSlope())
        {
            playerRigidbody.AddForce(moveDirection.normalized * moveSpeed * groundMovementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && onSlope())
        {
            playerRigidbody.AddForce(slopedMoveDirection.normalized * moveSpeed * groundMovementMultiplier, ForceMode.Acceleration);
        }
        else
        {
            playerRigidbody.AddForce(moveDirection.normalized * moveSpeed * airMovementMultiplier, ForceMode.Acceleration);
        }
    }


    private bool onSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hitSlope, playerHeight / 2 + 0.15f, layerMask))
        {
            if (hitSlope.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    private void Drag()
    {
        if (isGrounded)
        {
            playerRigidbody.drag = groundDrag;
        }
        else
        {
            playerRigidbody.drag = airDrag;
        }
    }
    

    private void Speed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded) // Remove me when using the NUSDK
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, accelerationSpeed * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, accelerationSpeed * Time.deltaTime);
        }
    }


    private void IKRigging()
    {
        if (useIK)
        {
            // Lock foot positions
            leftLegTarget.position = lastLeftLegPos;
            rightLegTarget.position = lastRightLegPos;

            lastLeftLegPos = leftLegTarget.position;
            lastRightLegPos = rightLegTarget.position;


            // Raycast ideal positions
            if (Physics.Raycast(leftRaycast.position, leftRaycast.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
            {
                leftLegIdeal.position = hit.point - stepOffset;
                if (Vector3.Distance(leftLegTarget.position, leftLegIdeal.position) > maxLegDistance)
                {
                    leftLegTarget.position = leftLegIdeal.position;
                    lastLeftLegPos = leftLegIdeal.position;
                }
            }
            if (Physics.Raycast(rightRaycast.position, rightRaycast.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
            {
                rightLegIdeal.position = hit.point - stepOffset;
                if (Vector3.Distance(rightLegTarget.position, rightLegIdeal.position) > maxLegDistance)
                {
                    rightLegTarget.position = rightLegIdeal.position;
                    lastRightLegPos = rightLegIdeal.position;
                }
            }
        }
    }
}

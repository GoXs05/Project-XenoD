using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Transform orientation;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;
    public float doubleJumpCount = 1f;
    public bool onSlope;
    private bool isADS;
    //bool isImpacting = false;
    //private float walkingFOV = 70f;
    //private float sprintingFOV = 100f;
    //public float currentFOV;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float adsWalkSpeed = 2f;
    [SerializeField] float adsSprintSpeed = 3f;
    [SerializeField] float acceleration = 10f;
    //private bool isSprinting;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Jumping")]
    float jumpForce = 18f;

    [Header("References")]
    [SerializeField] WallRun wallRun;
    [SerializeField] Camera cam;
    //[SerializeField] Rigidbody camRb;
    [SerializeField] PlayerSlide slide;
    [SerializeField] GameObject weapon;
    [SerializeField] Recoil recoilScript;
    [SerializeField] WeaponSway sway;
    private Gun gunScript;

    public Rigidbody rb;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    RaycastHit slopeHit; 

    float groundDrag = 6f;
    float airDrag = 1.5f;
    float wallLatchDrag = 100f;

    float horizontalMovement;
    float verticalMovement;

    float playerHeight = 3f; 

    Vector3 camTargetPos = new Vector3(0f, 0f, 0f);

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    float groundDistance = 0.4f;
    public bool isGrounded;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }



    void Update()
    {
        //Grounded Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Prevents sliding down slopes
        if (isGrounded || wallRun.isWallRunning || wallRun.isWallLatching)
        {
            rb.useGravity = false;
        }

        else
        {
            rb.useGravity = true;
        }

        //currentFOV = cam.fieldOfView;

        if (weapon.transform.Find("AssaultRifle").gameObject.activeSelf)
        {
            gunScript = weapon.transform.Find("AssaultRifle").GetComponent<Gun>();
        }

        else if (weapon.transform.Find("Pistol").gameObject.activeSelf)
        {
            gunScript = weapon.transform.Find("Pistol").GetComponent<Gun>();
        }

        //Input, drag, and movement controllers
        MyInput();
        ControlDrag();
        Movement();
        ControlSpeed();
        WallRunCheck();
        //VaultCheck();


        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        onSlope = OnSlope();

        //sway.currentSpeed = AllowWeaponSway() ? rb.velocity.magnitude : 0;


        //Resetting Camera Position
        //cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camTargetPos, 0.8f);
        
    }



    /*private bool AllowWeaponSway() 
    {
        if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
        {
            return true;
        }

        return false;
    }*/



    /*private void VaultCheck()
    {

        if (!wallRun.isWallRunning && !wallRun.isWallLatching && !isGrounded)
        {
            Vector3 dir = rb.velocity;
            Vector3 maxVaultPos = transform.position + Vector3.up * 1.5f;

            if (Physics.Raycast(maxVaultPos, dir, 2f, groundMask))
            {
                return;
            }

            Vector3 hoverPos = maxVaultPos + dir * 2;

            RaycastHit hit;
            if (!Physics.Raycast(hoverPos, Vector3.down, out hit, 3f, groundMask))
            {
                return;
            }

            Vector3 landPos = hit.point + (Vector3.up * playerHeight * 0.5f);

            transform.position = landPos;
            rb.velocity = dir * 0.4f;
        }
    }*/



    public bool WallRunCheck()
    {
        if (wallRun.wallLeft || wallRun.wallRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public bool OnSlope() 
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }



    void Movement()
    {
        //First Jump
        if (Input.GetKeyDown(jumpKey) && (isGrounded))
        {
            Jump();
        }

        //Double Jump
        if (Input.GetKeyDown(KeyCode.Q) && !isGrounded && doubleJumpCount > 0 && !WallRunCheck())
        {
            Jump();
            doubleJumpCount--; //Decrements double jump counter on midair jump
        }

        //Resets double jump counter on landing
        if (isGrounded)
        {
            doubleJumpCount = 1f;
        }
    }



    void MyInput()
    {
        //Gets WASD input
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        //Moves according to WASD input
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }



    void ControlSpeed()
    {
        
        if (Input.GetKey(sprintKey) && !gunScript.ads)
        {
            //isSprinting = true;
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            //cam.fieldOfView = Mathf.Lerp(currentFOV, sprintingFOV, 0.3f);
        }
        else if (WallRunCheck())
        {
            //cam.fieldOfView = Mathf.Lerp(currentFOV, wallRun.wallRunfov, 0.3f);
        }
        else if (Input.GetKey(sprintKey) && gunScript.ads)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, adsSprintSpeed, acceleration * Time.deltaTime);
        }
        else if (!Input.GetKey(sprintKey) && gunScript.ads)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, adsWalkSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            //cam.fieldOfView = Mathf.Lerp(currentFOV, walkingFOV, 0.3f);
        }
        /*if (Input.GetKeyUp(sprintKey))
        {
            isSprinting = false;
        }*/
    }



    void ControlDrag() 
    {
        if (isGrounded && !slide.isSlidingOnSlope) //sets ground drag
        {
            rb.drag = groundDrag;
        }
        else if (wallRun.isWallLatching)  //sets wall latch drag
        {
            rb.drag = wallLatchDrag;
        }
        else if (slide.isSlidingOnSlope) //sets sliding drag
        {
            rb.drag = slide.slideDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }



    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //cam.transform.localPosition = new Vector3(0f, -0.5f, 0f);
        //StartCoroutine(JumpImpulseDelay());
        //camRb.AddForce(new Vector3(0f, -0.05f, 0f), ForceMode.Impulse);

    }



    private IEnumerator JumpImpulseDelay()
    {
        yield return new WaitForSeconds(0.25f);
        cam.transform.localPosition = new Vector3(0f, 0f, 0f);
    }



    private void FixedUpdate()
    {
        MovePlayer();
    }



    void MovePlayer()
    {
        //WASD Movement with forces
        if (isGrounded && !onSlope) //ground drag
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && onSlope) //slope drag
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        
        else if (!isGrounded) //air drag
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform orientation;

    [Header("Detection")]
    [SerializeField] private float wallDistance = 1.5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;

    [Header("Wall Running & Latching")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;
    public bool isWallLatching; 
    private bool canWallLatch = true;
    public bool isWallRunning = false;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] public float wallRunfov;
    //float currentFOV;
    [SerializeField] private float wallRunfovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt { get; private set; }

    public bool wallLeft = false;
    public bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private Rigidbody rb;

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        //Debug.DrawRay(transform.position, -orientation.right, Color.red, 2f, false);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void Update()
    {

        //currentFOV = cam.fieldOfView;

        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                if (Input.GetKey(KeyCode.Mouse1) && canWallLatch)
                {
                    StartWallLatch();
                }
                else
                {
                    StartWallRun();
                }
            }
            else if (wallRight)
            {
                if (Input.GetKey(KeyCode.Mouse1) && canWallLatch)
                {
                    StartWallLatch();
                }
                else
                {
                    StartWallRun();
                }
            }
            else
            {
                StopWallRun();
            }
        }
        else if (!Input.GetMouseButton(1))
        {
            StopWallRun();
        }
    }

    void StartWallRun()
    {
        rb.useGravity = false;
        isWallLatching = false;
        isWallRunning = true;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        SetWallFOV();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            WallJump();
        }
    }



    void SetWallFOV()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        } 
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }
    }



    void WallJump()
    {

        isWallRunning = false;
        isWallLatching = false;

        if (wallLeft)
        {
            Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 70, ForceMode.Force);
        }
        else if (wallRight)
        {
            Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); 

            rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 70, ForceMode.Force);
        }
    }



    void StartWallLatch()
    {
        rb.useGravity = false;
        isWallLatching = true;
        isWallRunning = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(WallLatchJump());
        }
    }



    IEnumerator WallLatchJump()
    {
        isWallLatching = false;
        canWallLatch = false;
        
        rb.drag = 6f;
        WallJump();

        yield return new WaitForSeconds(0.2f); 
        
        canWallLatch = true;
    }



    void StopWallRun()
    {
        rb.useGravity = true;
        isWallLatching = false;
        isWallRunning = false;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
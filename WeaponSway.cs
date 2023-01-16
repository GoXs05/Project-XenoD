using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    [Header("Sway Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;
    [SerializeField] private Recoil recoilScript;
    //private Rigidbody rb;

    private Quaternion refRotation;
    private Vector3 refPos;

    private float xRotation;
    private float yRotation;

    private Gun gunScript;
    //public float movementMagnitude;

    private Vector3 impulse;

    private void Start() 
    {
        refPos = transform.position;

        //rb = transform.GetComponent<Rigidbody>();

        //impulse = new Vector3(200f, 100f, 0f);
    }

    private void Update()
    {
        //Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityMultiplier;

		Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
		Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

		Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);


        //Get gunScript
        gunScript = recoilScript.gunScript;
        //movementMagnitude = gunScript.movementMagnitude;


        //Movement Sway
        /*if (!recoilScript.ads)
        {   
            //MoveSway();
        }*/
        
    }



    /*private void MoveSway()
    {
        StartCoroutine(SwayDelay());
        impulse.x *= -1;
    }



    private IEnumerator SwayDelay()
    {
        //rb.AddForce(impulse, ForceMode.Impulse);
        Debug.Log("sway");
        yield return new WaitForSeconds(1f);
    }*/
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSway : MonoBehaviour
{
    public float swayIntensityX;
    public float swayIntensityY;
    public float maxSway;
    public float minSway;

    public BobOverride[] bobOverrides;

    private Transform weapon;

    [HideInInspector]
    public float currentSpeed;

    private float currentTimeX;
    private float currentTimeY;
    
    private float xPos;
    private float yPos;

    private Vector3 smoothV;




    private void Update() 
    {

        if (transform.Find("AssaultRifle").gameObject.activeSelf)
        {
            weapon = transform.Find("AssaultRifle");
        }

        else if (transform.Find("Pistol").gameObject.activeSelf)
        {
            weapon = transform.Find("Pistol");
        }

        else if (transform.Find("Katana").gameObject.activeSelf)
        {
            weapon = transform.Find("Katana");
        }

        //Debug.Log(currentSpeed);


        foreach (BobOverride bob in bobOverrides) 
        {
            if (currentSpeed >= bob.minSpeed && currentSpeed <= bob.maxSpeed) 
            {
                //Debug.Log(bob);

                float bobMultiplier = (currentSpeed == 0) ? 1 : currentSpeed;

                currentTimeX += bob.speedX / 10 * Time.deltaTime * bobMultiplier;
                currentTimeY += bob.speedY / 10 * Time.deltaTime * bobMultiplier;

                xPos = bob.bobX.Evaluate(currentTimeX) * bob.intensityX;
                yPos = bob.bobY.Evaluate(currentTimeY) * bob.intensityY;
            }
        }

        float xSway = -Input.GetAxis("Mouse X") * swayIntensityX;
        float ySway = -Input.GetAxis("Mouse Y") * swayIntensityY;

        xSway = Mathf.Clamp(xSway, minSway, maxSway);
        ySway = Mathf.Clamp(ySway, minSway, maxSway);

        xPos += xSway;
        yPos += ySway;
    }




    private void FixedUpdate() 
    {
        Vector3 target = new Vector3(xPos, yPos, 0);
        Vector3 desiredPos = Vector3.SmoothDamp(weapon.localPosition, target, ref smoothV, 0.1f);
        weapon.localPosition = desiredPos;
    }




    [System.Serializable]
    public struct BobOverride 
    {
        public float minSpeed;
        public float maxSpeed;

        [Header("X Settings")]
        public float speedX;
        public float intensityX;
        public AnimationCurve bobX;

        [Header("Y Settings")]
        public float speedY;
        public float intensityY;
        public AnimationCurve bobY;
    }
}*/
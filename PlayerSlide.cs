using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject weaponHolder;

    public float slideDrag = 4f;

    public bool isSlidingOnSlope = false;
    public bool isSliding = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {

        if (!PauseMenu.GameIsPaused)
        {

            if (Input.GetKeyDown(KeyCode.C))
            {
                StartSlide();
            }
            if (Input.GetKey(KeyCode.C))
            {
                CheckSlope();
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                StopSlide();
            }
            
        }

    }



    void CheckSlope()
    {
        if (playerMovement.onSlope)
        {
            isSlidingOnSlope = true;
        }
        else if (!playerMovement.onSlope)
        {
            isSlidingOnSlope = false;
        }
    }



    void StartSlide()
    {

        isSliding = true;

        transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
        weaponHolder.transform.localScale = new Vector3(transform.localScale.x, 2f, transform.localScale.z);
    }



    void StopSlide() 
    {
        isSlidingOnSlope = false;
        isSliding = false;

        transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z); 
        weaponHolder.transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{

    [SerializeField] PlayerMovement movementScript;
    [SerializeField] private Animator animator;

    public bool isSprinting;

    void Start()
    {
        
    }



    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftShift)) isSprinting = true;
        else isSprinting = false;

        
        if (movementScript.isGrounded && (movementScript.rb.velocity.magnitude > 1.5f))
        {

            animator.SetBool("isMoving", true);

            if (!isSprinting)
            {
                animator.SetFloat("speedMultiplier", 0.6f);
            }

            else
            {
                animator.SetFloat("speedMultiplier", 0.8f);
            }

        }

        else 
        {
            animator.SetBool("isMoving", false);
        }

    }
}

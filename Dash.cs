using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{

    private Rigidbody rb;

    private float dashImpulse = 9f;
    private bool canDash = true;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform cam;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StartDash();
        }

        if (Input.GetMouseButton(1) && animator.GetBool("heavyAtk1") && canDash)
        {
            StartDash();
        }
    }



    private void StartDash()
    {
        canDash = false;

        if (rb.velocity.magnitude < 0.1f)
        {
            rb.AddForce(cam.transform.forward * dashImpulse * 5f, ForceMode.Impulse);
        }

        else
        {
            rb.AddForce(rb.velocity * dashImpulse, ForceMode.Impulse);
        }

        StartCoroutine(DashCooldown());
    }



    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(4f);
        canDash = true;
    }
}

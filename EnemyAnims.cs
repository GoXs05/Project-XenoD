using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnims : MonoBehaviour
{

    [SerializeField] public Animator animator;
    [SerializeField] public Animator gunAnims;
    [SerializeField] EnemyAI AI;

    private bool canShoot = true;
    private Vector3 targetBigCannonPos;

    //Animation References
    //[Header("Animation References")]

    /*[SerializeField] private GameObject BigCannon01L;
    [SerializeField] private GameObject BigCannon01R;
    [SerializeField] private GameObject BigCannonStretcher01L;
    [SerializeField] private GameObject BigCannonStratcher01R;
    [SerializeField] private GameObject BigCannons;
    [SerializeField] private GameObject UpperBody;*/

    void Start()
    {
        //animator = transform.GetComponent<Animator>();
        //targetBigCannonPos = new Vector3 (0f, 0f, 0f);
    }



    void Update()
    {
        if (AI.chasing)
        {
            animator.enabled = true;
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", true);
            canShoot = false;
        }

        else
        {
            animator.SetBool("isWalking", false);
            StartCoroutine(AnimatorDisable());

            if (AI.attacking /*&& !animator.isActiveAndEnabled == true*/)
            {
                animator.SetBool("isAttacking", true);
                canShoot = true;
                AttackManager();
            }
        }

        //BigCannons.transform.localPosition = Vector3.Lerp(BigCannons.transform.localPosition, targetBigCannonPos, 2f * (Time.time % 0.5f));

    }



    private IEnumerator AnimatorDisable()
    {
        yield return new WaitForSeconds(0.5f);
        animator.enabled = false;
    }



    private void AttackManager()
    {

        BC1Shoot();

    }



    //Shoots Big Cannon 1
    private void BC1Shoot()
    {
        if (canShoot)
        {

            gunAnims.SetBool("shootingBC", true);

            Debug.Log("shooting");
            
            //canShoot = false;

            //Rigidbody rb = BigCannons.GetComponent<Rigidbody>();
            //rb.AddForce(new Vector3(-BigCannons.transform.rotation.x, 0f, -BigCannons.transform.rotation.z), ForceMode.Impulse);
            //rb.AddRelativeForce(Vector3.forward);
            //rb.AddRelativeForce(new Vector3(0f, 0f, -0.15f), ForceMode.Impulse);
            //Debug.Log("Shot");
            //Debug.DrawRay(UpperBody.transform.position, UpperBody.transform.forward, Color.green, 0.5f);

            //StartCoroutine(ShootDelay());

        }

        else
        {
            gunAnims.SetBool("shootingBC", false);

            Debug.Log("not shooting");
        }

    }



    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds (0.5f);
        canShoot = true;
    }

}

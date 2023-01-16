using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    private bool canAttack;

    public bool canHvyAtkCombo;
    public bool isAttacking;

    public bool lightAtk;
    public bool heavyAtk;

    [SerializeField] private Animator animator;
    [SerializeField] private KatanaAttackDelays delay;
    [SerializeField] private GameObject shield;

    public bool canDmg;

    private float timeCount;



    void Start()
    {
        canAttack = true;
        isAttacking = false;
        canHvyAtkCombo = false;
        canDmg = true;
    }




    void Update()
    {
        if (transform.gameObject.activeSelf)
        {
            AttackManager();

            //Checks if any animations should be playing or not
            if (animator.GetBool("lightAtk1") || animator.GetBool("lightAtk2") || animator.GetBool("heavyAtk1") || animator.GetBool("heavyAtk2") || animator.GetBool("heavyAtk3"))
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }
        }
        
        timeCount += 1 * Time.deltaTime;

        if (timeCount > 4)
        {
            animator.SetBool("lightAtk1", false);
            animator.SetBool("lightAtk2", false);
            animator.SetBool("heavyAtk1", false);
            animator.SetBool("heavyAtk2", false);
            animator.SetBool("heavyAtk3", false);
        }

    }



    void AttackManager()
    {
        if(canAttack && Input.GetMouseButtonDown(0))
        {
            LightAttack();
        }

        else if (canAttack && Input.GetMouseButtonDown(1))
        {
            HeavyAttack();
        }

        else if (Input.GetKey(KeyCode.F))
        {
            Shield();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            canAttack = true;
            animator.SetBool("isShielding", false);
            shield.SetActive(false);
        }
    }



    void LightAttack()
    {


        if (!animator.GetBool("lightAtk1")) 
        {
            lightAtk = true;
            animator.SetBool("lightAtk1", true);
            timeCount = 0;
            StartCoroutine(delay.LightAtk1Delay());
        }

        else if (animator.GetBool("lightAtk1"))
        {
            lightAtk = true;
            animator.SetBool("lightAtk2", true);
            timeCount = 0;
            StartCoroutine(delay.LightAtk2Delay());
        }

        
    }



    void HeavyAttack()
    {


        if (animator.GetBool("lightAtk1") && animator.GetBool("lightAtk2"))
        {
            heavyAtk = true;
            animator.SetBool("heavyAtk1", true);
            timeCount = 0;
            StartCoroutine(delay.HeavyAtk1Delay());
        }

        else if (!isAttacking)
        {
            heavyAtk = true;
            animator.SetBool("heavyAtk2", true);
            canHvyAtkCombo = false;
            timeCount = 0;
            StartCoroutine(delay.HeavyAtk2Delay());
            StartCoroutine(delay.HeavyAtkComboDelay());
        }

        else if (animator.GetBool("heavyAtk2") && canHvyAtkCombo)
        {
            heavyAtk = true;
            canHvyAtkCombo = false;
            animator.SetBool("heavyAtk3", true);
            timeCount = 0;
            StartCoroutine(delay.HeavyAtk3Delay());
        }

    }



    private void Shield()
    {
        canAttack = false;
        isAttacking = false;
        animator.SetBool("isShielding", true);
        shield.SetActive(true);
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Delays for Katana animations and states
public class KatanaAttackDelays : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Katana katana;



    public IEnumerator LightAtk1Delay()
    {
        yield return new WaitForSeconds(0.9f);
        if (!Input.GetMouseButtonDown(0))
        {
            animator.SetBool("lightAtk1", false);
            katana.lightAtk = false;
        }
    }



    public IEnumerator LightAtk2Delay()
    {
        yield return new WaitForSeconds(0.9f);
        if (!Input.GetMouseButtonDown(0) || !Input.GetMouseButtonDown(1))
        {
            animator.SetBool("lightAtk2", false);
            katana.lightAtk = false;
        }
    }



    public IEnumerator HeavyAtk1Delay()
    {
        yield return new WaitForSeconds(1.2f);
        animator.SetBool("heavyAtk1", false);
        katana.heavyAtk = false;
    }



    public IEnumerator HeavyAtk2Delay()
    {
        yield return new WaitForSeconds(1.4f);
        animator.SetBool("heavyAtk2", false);
        katana.heavyAtk = false;
    }



    public IEnumerator HeavyAtk3Delay()
    {
        yield return new WaitForSeconds(1.4f);
        animator.SetBool("heavyAtk3", false);
        katana.heavyAtk = false;
    }


    public IEnumerator HeavyAtkComboDelay()
    {
        yield return new WaitForSeconds(0.8f);
        katana.canHvyAtkCombo = true;
    }



}

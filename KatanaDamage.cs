using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaDamage : MonoBehaviour
{

    private Collider col;
    [SerializeField] Transform cam;

    [SerializeField] private Katana katanaScript;

    void Start()
    {
        col = transform.GetComponent<Collider>();
    }



    //Detects if collider on katana is colliding with another trigger collider
    private void OnTriggerEnter(Collider other) 
    {
        if (katanaScript.isAttacking && katanaScript.canDmg)
        {
            if (katanaScript.lightAtk)
            {
                IDamageable damageable = other.gameObject.transform.GetComponent<IDamageable>();
                damageable?.TakeDamage(20f);
                katanaScript.canDmg = false;
                StartCoroutine(LightAttackDmgDelay());
            }

            else
            {
                IDamageable damageable = other.gameObject.transform.GetComponent<IDamageable>();
                damageable?.TakeDamage(80f);
                katanaScript.canDmg = false;
                StartCoroutine(HeavyAttackDmgDelay());
            }

        }
    }


    private IEnumerator LightAttackDmgDelay()
    {
        yield return new WaitForSeconds(0.3f);
        katanaScript.canDmg = true;
    }



    private IEnumerator HeavyAttackDmgDelay()
    {
        yield return new WaitForSeconds(0.9f);
        katanaScript.canDmg = true;
    }



    private void OnTriggerExit(Collider other) 
    {
        //Nothing for now
    }


    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableCrate : MonoBehaviour, IDamageable
{

    private float health = 10f;
    [SerializeField] GameObject energyOrb;
    [SerializeField] GameObject healthOrb;


    public void TakeDamage(float damage)
    {

        health -= damage;

        if (health <= 0)
        {
            float energyChance = Random.value;
            float healthChance = Random.value;

            Vector3 force = new Vector3 (Random.value * 10, Random.value * 10, Random.value * 10);

            if (energyChance > 0.2 && healthChance > 0.5)
            {
                ;
            }
            if (energyChance < 0.2)
            {
                GameObject EO = Instantiate(energyOrb, transform.position, transform.rotation);
                EO.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            }
            if (healthChance < 0.5)
            {
                GameObject HO = Instantiate(healthOrb, transform.position, transform.rotation);
                HO.GetComponent<Rigidbody>().AddForce(-force, ForceMode.Impulse);
            }
            
            Destroy(gameObject, 0f);
            
        }
    }
    

}

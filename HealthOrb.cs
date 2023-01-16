using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    public float healthOrbHealth = 15f;

    void Awake() 
    {
        StartCoroutine(HealthOrbDespawn());
    }

    private IEnumerator HealthOrbDespawn()
    {
        yield return new WaitForSeconds(7f);

        Destroyer();
    }

    public void Destroyer()
    {
        Destroy(gameObject);
    }
}

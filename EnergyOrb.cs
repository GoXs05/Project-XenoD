using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyOrb : MonoBehaviour
{

    public float energyOrbEnergy = 15f;

    void Awake() 
    {
        StartCoroutine(EnergyOrbDespawn());
    }

    private IEnumerator EnergyOrbDespawn()
    {
        yield return new WaitForSeconds(7f);

        Destroyer();
    }

    public void Destroyer()
    {
        Destroy(gameObject);
    }

}

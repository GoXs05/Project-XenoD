using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAbilities : MonoBehaviour
{
    //References
    private Gun gunScript;

    //Energy
    public float energy = 300f;
    public float maxEnergy = 350f;

    //Health and Shields
    public float playerHealth = 1000f;
    public float maxPlayerHealth = 1200f;
    public float playerShields = 0f;
    public float shieldsEnergy = 75f;
    bool canUseShields = true;


    void Start()
    {
        //nothing for now
    }

    void Update()
    {
        EnergyShields();

        EnergySetter();
        MaxHealthSetter();
    }



    void MaxHealthSetter()
    {
        if (playerHealth > maxPlayerHealth)
        {
            playerHealth = maxPlayerHealth;
        }
    }



    void EnergySetter()
    {
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
    }



    void EnergyShields()
    {
        //Gives player shields if conditions are met
        if (Input.GetKeyDown(KeyCode.X) && canUseShields && energy >= shieldsEnergy)
        {
            canUseShields = false;
            playerShields += 125f;
            energy -= shieldsEnergy;
        }

        //Checks to see if player can use shields
        if (playerShields == 0f)
        {
            canUseShields = true;
        }
    }



    public void TakeDamage(float damage)
    {
        if (playerShields > 0f)
        {
            playerShields -= damage;
        }
        else
        {
            playerHealth -= damage;
        }

        HealthAndShieldSetter();
    }

    private void HealthAndShieldSetter()
    {
        if (playerHealth < 0f)
        {
            playerHealth = 0f;
            //Death logic to be implemented later
        }


        if (playerShields < 0f)
        {
            playerShields = 0f;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {

        if (other.gameObject.layer == 7)
        {
            if (energy < maxEnergy)
            {
                energy += other.gameObject.GetComponent<EnergyOrb>().energyOrbEnergy;
            }            

            else
            {
                Debug.Log("Max Energy Reached");
            }

            other.gameObject.GetComponent<EnergyOrb>().Destroyer();

        }

        else if (other.gameObject.layer == 11)
        {
            if (playerHealth < maxPlayerHealth)
            {
                playerHealth += other.gameObject.GetComponent<HealthOrb>().healthOrbHealth;
            }

            else
            {
                Debug.Log("Max Health Reached");
            }

            other.gameObject.GetComponent<HealthOrb>().Destroyer();
        }

        
    }


}
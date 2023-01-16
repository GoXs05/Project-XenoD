using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public static Action shootInput;
    public static Action reloadInput;
    private bool hasGun;

    [Header("Keybinds")]
    [SerializeField] private KeyCode reloadKey;
    [SerializeField] private GameObject weapon;




    private void Update()
    {

        if (weapon.transform.Find("AssaultRifle").gameObject.activeSelf)
        {
            hasGun = true;

            if (Input.GetMouseButton(0))
            {
                shootInput?.Invoke();
            }
        }

        else if (weapon.transform.Find("Pistol").gameObject.activeSelf)
        {
            hasGun = true;

            if (Input.GetMouseButtonDown(0))
            {
                shootInput?.Invoke();
            }
        }

        else if (weapon.transform.Find("Katana").gameObject.activeSelf)
        {
            hasGun = false;

            if (Input.GetMouseButtonDown(0))
            {
                
            }
        }

        if (hasGun && Input.GetKeyDown(reloadKey))
        {
            reloadInput?.Invoke();
        }
    }
}

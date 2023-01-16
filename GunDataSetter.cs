using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDataSetter : MonoBehaviour
{
    [SerializeField] GunData AR;
    [SerializeField] GunData Pistol;
    

    void Start()
    {
        AR.magSize = 30;
        AR.currentAmmo = 30;
        AR.damage = 25f;

        Pistol.fireRate = 85f;
        Pistol.damage = 65f;
    }


}

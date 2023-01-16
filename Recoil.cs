using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //Hipfire recoil angles
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    //ADS recoil angles
    public float adsRecoilX;
    public float adsRecoilY;
    public float adsRecoilZ;

    //Hipfire recoil positions


    //ADS recoil positions


    //ADS Bools
    public bool ads;
    

    //Settings
    public float snappiness;
    public float returnSpeed;

    public Gun gunScript;
    public Katana katanaScript;


    void Start()
    {
        
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);

        if (transform.Find("WeaponHolder/AssaultRifle").gameObject.activeSelf)
        {
            gunScript = transform.Find("WeaponHolder/AssaultRifle").GetComponent<Gun>();
        }
        else if (transform.Find("WeaponHolder/Pistol").gameObject.activeSelf)
        {
            gunScript = transform.Find("WeaponHolder/Pistol").GetComponent<Gun>();
        }
        else if (transform.Find("WeaponHolder/Katana").gameObject.activeSelf)
        {
            katanaScript = transform.Find("WeaponHolder/Katana").GetComponent<Katana>();
        }
        else
        {
            Debug.Log("null weapon in recoil script");
        }

        if (gunScript != null)
        {
            ads = gunScript.ads;
            GetGunRecoilData();
        }
        else if (katanaScript != null)
        {
            
        }
        
    }

    public Gun GetGunScript()
    {
        return gunScript;
    }



    private void GetGunRecoilData()
    {
        recoilX = gunScript.recoilX;
        recoilY = gunScript.recoilY;
        recoilZ = gunScript.recoilZ;

        adsRecoilX = gunScript.adsRecoilX;
        adsRecoilY = gunScript.adsRecoilY;
        adsRecoilZ = gunScript.adsRecoilZ;

        snappiness = gunScript.snappiness;
        returnSpeed = gunScript.returnSpeed;
    }



    public void RecoilFire()
    {
        if (Input.GetMouseButton(1))
        {
            targetRotation += new Vector3(adsRecoilX, Random.Range(-adsRecoilY, adsRecoilY), Random.Range(-adsRecoilZ, adsRecoilZ));
        }
        else
        {
            targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun" , menuName = "Weapon/Gun")]

public class GunData : ScriptableObject 
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    public float hipfireSpread;
    public float adsSpread;

    public bool isAutomatic;
    public bool isSemiAutomatic;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public int totalAmmo;
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;

    [Header("Hipfire Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    [Header("ADS Recoil")]
    public float adsRecoilX;
    public float adsRecoilY;
    public float adsRecoilZ;

    [Header("Hipfire Gun Recoil Angles")]
    public float gunRecoilX;
    public float gunRecoilY;
    public float gunRecoilZ;

    [Header("ADS Gun Recoil Angles")]
    public float adsGunRecoilX;
    public float adsGunRecoilY;
    public float adsGunRecoilZ;

    [Header("Hipfire Gun Recoil Positions")]
    public float gunRecoilPosX;
    public float gunRecoilPosY;
    public float gunRecoilPosZ;

    [Header("ADS Gun Recoil Positions")]
    public float adsGunRecoilPosX;
    public float adsGunRecoilPosY;
    public float adsGunRecoilPosZ;

    [Header("Settings")]
    public float snappiness;
    public float returnSpeed;
    public float gunPosSnappiness;
    public float gunPosReturnSpeed;

    [Header("Weapon Charge Gimmicks")]
    public int magSizeFactor;
    public float headshotmultiplierFactor;
    public float spreadReductionFactor;
    public float fireRateFactor;
    public float weaponChargeDMGFactor;

}

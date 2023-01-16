using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //Positions
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    public float movementMagnitude;

    Vector3 initialMovement;
    Vector3 currentMovement;

    [SerializeField] GunData gunData;
    [SerializeField] Transform recoilCam;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject bulletHole;
    [SerializeField] Camera cam;
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem sparks;
    [SerializeField] Material gunMaterial;
    [SerializeField] Animator animator;

    RaycastHit hit;

    Recoil Recoil_Script;

    float timeSinceLastShot;

    //Hipfire recoil
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    public float adsRecoilX;
    public float adsRecoilY;
    public float adsRecoilZ;

    //Settings
    public float snappiness;
    public float returnSpeed;

    //ADS values
    private Vector3 curPos;
    private Vector3 hipfirePos;
    private Vector3 adsPos;
    private float adsFOV = 60f;
    private float curFOV;
    private float defaultFOV = 70f;
    private bool canSwitchADS = true;
    private bool hipfiring = true;
    public bool ads = false;

    //References
    private PlayerMovement playerMoveScript;
    private WallRun wallRunScript;
    private PlayerSlide playerSlideScript;
    private PlayerAbilities playerAbilitiesScript;

    //Weapon Charging
    public bool weaponsAreCharged = false;
    private bool weaponChargeEnergyDecrementing = false;
    private bool canChargeWeapons = true;
    //private Vector3 glow;
    //private Vector3 noGlow;
    //GameObject ARChargeObjects;
    //GameObject ARChargeRings;
    //GameObject PistolChargeObjects;
    //GameObject PistolChargeRings;

    //Weapon Delays
    private bool isSwitching;

    //Checks if not reloading and time since last shot > minimum time between shots
    private bool CanShoot() => !isSwitching && !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);



    private void Start() 
    {
        gunMaterial.SetColor("_EmissionColor", new Color (0f, 0.66f, 1.0f, 1.0f));

        //glow = new Vector3 (gunMaterial.GetColor("_EmissionColor").r, gunMaterial.GetColor("_EmissionColor").g, gunMaterial.GetColor("_EmissionColor").b);
        //noGlow = glow * 6f;

        initialMovement = player.transform.position;

        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

        Recoil_Script = transform.parent.parent.GetComponent<Recoil>();

        hipfirePos = transform.localPosition;
        adsPos = new Vector3 (-0.18f, 0.08f, 0f);

        SetGunRecoilData();

        playerMoveScript = transform.parent.parent.parent.parent.GetComponent<PlayerMovement>();
        wallRunScript = transform.parent.parent.parent.parent.GetComponent<WallRun>();
        playerSlideScript = transform.parent.parent.parent.parent.GetComponent<PlayerSlide>();
        playerAbilitiesScript = transform.parent.parent.parent.parent.GetComponent<PlayerAbilities>();

        if (gunData.name == "Assault Rifle")
        {
            //Objects = transform.Find("AR Gun/Charge Glow Objects").gameObject;
            //ARChargeRings = ARChargeObjects.transform.Find("Charge Glow Rings").gameObject;
        }

        else if (gunData.name == "Pistol")
        {
            //PistolChargeObjects = transform.Find("Pistol Gun/Charge Glow Objects").gameObject;
            //PistolChargeRings = PistolChargeObjects.transform.Find("Charge Glow Rings").gameObject;
        }

    }



    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf && gunData.totalAmmo > 0 && !PauseMenu.GameIsPaused)
        {
            StartCoroutine(Reload());
        }
    }



    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        if (gunData.totalAmmo > gunData.magSize)
        {
            gunData.currentAmmo = gunData.magSize; 
            gunData.totalAmmo -= gunData.magSize;
        }
        
        if (gunData.totalAmmo < gunData.magSize)
        {
            gunData.currentAmmo = gunData.totalAmmo;
            gunData.totalAmmo = 0;
        }

        gunData.reloading = false;
    }



    public void Shoot()
    {
        if (gunData.currentAmmo > 0 && !PauseMenu.GameIsPaused)
        {
            if (CanShoot())
            {
                Vector3 forwardVector = Vector3.forward;
                float deviation;
                float angle;
                
                if (hipfiring)
                {
                    if (movementMagnitude <= 0.10)
                    {
                        deviation = Random.Range(0f, gunData.hipfireSpread);
                    }
                    else
                    {
                        deviation = Random.Range(0f, gunData.hipfireSpread * movementMagnitude);
                    }
                }
                else
                {
                    if (movementMagnitude <= 0.10)
                    {
                        deviation = Random.Range(0f, gunData.hipfireSpread);
                    }
                    else
                    {
                        deviation = Random.Range(0f, gunData.hipfireSpread * movementMagnitude);
                    }
                }

                angle = Random.Range(0f, 360f);
                forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
                forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
                forwardVector = recoilCam.transform.rotation * forwardVector;
                

                if (Physics.Raycast(recoilCam.position, forwardVector, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);
                    hit = hitInfo;
                    if (transform.gameObject.activeSelf)
                    {
                        StartCoroutine(BulletHole());
                    }
                    
                    
                }
                //sfx
                Recoil_Script.RecoilFire();
                muzzleFlash.Play();
                

                //ammo and shooting logic
                gunData.currentAmmo--;
                timeSinceLastShot = 0f;
                GunRecoil();
            }

        }
    }



    //Creating & Destroying Bullet Hole Objects
    private IEnumerator BulletHole()
    {
        GameObject bH = Instantiate (bulletHole, hit.point, Quaternion.LookRotation(hit.normal));

        yield return new WaitForSeconds(0.05f);

        if (hit.transform == null)
        {
            Destroy(bH, 0f);
        }
        else
        {
            Destroy(bH, .3f);
        }
    }



    //
    private void Update()
    {

        currentMovement = player.transform.position;

        movementMagnitude = (((initialMovement - currentMovement).magnitude)) * 20f;

        initialMovement = currentMovement;

        timeSinceLastShot += Time.deltaTime;

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, gunData.gunPosReturnSpeed * Time.deltaTime);
        

        transform.localRotation = Quaternion.Euler(currentRotation);


        if (!ads)
        {
            currentPosition = Vector3.Slerp(currentPosition, targetPosition, gunData.gunPosSnappiness * Time.fixedDeltaTime);
            transform.localPosition = currentPosition;
        }
        else if (ads)
        {
            currentPosition = Vector3.Slerp(currentPosition, targetPosition + adsPos, gunData.gunPosSnappiness * Time.fixedDeltaTime);
            transform.localPosition = currentPosition;
        }
        

        curPos = transform.localPosition;

        curFOV = cam.fieldOfView;

        if (Input.GetMouseButton(1) && canSwitchADS && (playerMoveScript.isGrounded || wallRunScript.isWallLatching) && !playerSlideScript.isSliding)
        {
            hipfiring = false;
            ads = true;

            playerMoveScript.moveSpeed /= 3;

            transform.localPosition = Vector3.Lerp(curPos, adsPos, 0.25f * Time.deltaTime * 200);
            cam.fieldOfView = Mathf.Lerp(curFOV, adsFOV, 0.25f * Time.deltaTime * 400);

            canSwitchADS = false;

            StartCoroutine(ADSCooldown());
        }

        if ((Input.GetMouseButtonUp(1) || playerSlideScript.isSliding) && ads)
        {
            hipfiring = true;
            ads = false;

            playerMoveScript.moveSpeed *= 3;

            StartCoroutine(Delay());
        }

        WeaponCharge();

    }



    //Weapon Charge Management and Emission
    private void WeaponCharge()
    {
        if (Input.GetKeyDown(KeyCode.V) && canChargeWeapons)
        {
            canChargeWeapons = false;
            StartCoroutine(ChargeDelay());
            
            if (!weaponsAreCharged)
            {
                weaponChargeEnergyDecrementing = false;
                WeaponChargeStats();
                weaponsAreCharged = true;

                //gunMaterial.SetColor("_EmissionColor", new Color (Mathf.Lerp(gunMaterial.GetColor("_EmissionColor").r , gunMaterial.GetColor("_EmissionColor").r * 6f, 1.0f), Mathf.Lerp(gunMaterial.GetColor("_EmissionColor").g , gunMaterial.GetColor("_EmissionColor").g * 6f, 1.0f), Mathf.Lerp(gunMaterial.GetColor("_EmissionColor").b , gunMaterial.GetColor("_EmissionColor").b * 6f, 1.0f), Mathf.Lerp(gunMaterial.GetColor("_EmissionColor").a , gunMaterial.GetColor("_EmissionColor").a * 6f, 1.0f)));
            }

            else if (weaponsAreCharged)
            {
                WeaponUnchargeStats();
                weaponsAreCharged = false;
                CancelInvoke("WeaponEnergyDecrement");

                //gunMaterial.SetColor("_EmissionColor", new Color (Mathf.Lerp(gunMaterial.GetColor("_EmissionColor").r , gunMaterial.GetColor("_EmissionColor").r / 6f, 1.0f), Mathf.Lerp(gunMaterial.GetColor("_EmissionColor").g , gunMaterial.GetColor("_EmissionColor").g / 6f, 1.0f), Mathf.Lerp(gunMaterial.GetColor("_EmissionColor").b , gunMaterial.GetColor("_EmissionColor").b / 6f, 1.0f), Mathf.Lerp(gunMaterial.GetColor("_EmissionColor").a , gunMaterial.GetColor("_EmissionColor").a / 6f, 1.0f)));
            }
            
        }

        if (weaponsAreCharged)
        {
            
            while (gunMaterial.GetColor("_EmissionColor").a < 6)
            {
                Debug.Log("brightening");
                //gunMaterial.SetColor("_EmissionColor", new Color (0f, Mathf.Lerp(0.66f, 3.96f, Time.deltaTime), Mathf.Lerp(1.0f, 6.0f, Time.deltaTime), Mathf.Lerp(1.0f, 6.0f, Time.deltaTime)));
                gunMaterial.SetColor("_EmissionColor", gunMaterial.GetColor("_EmissionColor") * new Color (1.01f, 1.01f, 1.01f, 1.01f));
                //gunMaterial.SetColor("_EmissionColor", gunMaterial.GetColor("_EmissionColor") * (1.0001f));
                //StartCoroutine(ChargeGlowUp());
                //gunMaterial.SetColor("_EmissionColor", new Color (gunMaterial.GetColor("_EmissionColor").r, gunMaterial.GetColor("_EmissionColor").g, gunMaterial.GetColor("_EmissionColor").b, gunMaterial.GetColor("_EmissionColor").a + (Time.deltaTime)));
            }
        }

        else
        {
            
            while (gunMaterial.GetColor("_EmissionColor").a > 0.5)
            {
                Debug.Log("dimming");
                gunMaterial.SetColor("_EmissionColor", gunMaterial.GetColor("_EmissionColor") * new Color (0.01f, 0.01f, 0.01f, 0.01f));
                
                //StartCoroutine(ChargeGlowDown());
                //gunMaterial.SetColor("_EmissionColor", new Color (gunMaterial.GetColor("_EmissionColor").r, gunMaterial.GetColor("_EmissionColor").g, gunMaterial.GetColor("_EmissionColor").b, gunMaterial.GetColor("_EmissionColor").a - (Time.deltaTime)));
            }
        }

        WeaponChargeEnergy();
    
    }


    private IEnumerator ChargeGlowUp()
    {
        yield return new WaitForSeconds(0.05f);
        gunMaterial.SetColor("_EmissionColor", gunMaterial.GetColor("_EmissionColor") * (1.1f));
    }



    private IEnumerator ChargeGlowDown()
    {
        yield return new WaitForSeconds(0.05f);
        gunMaterial.SetColor("_EmissionColor", gunMaterial.GetColor("_EmissionColor") / (1.1f));
    }



    private IEnumerator ChargeDelay()
    {
        yield return new WaitForSeconds(0.25f);
        canChargeWeapons = true;
    }

    private void WeaponChargeEnergy()
    {
        if (weaponsAreCharged)
        {
            if (!weaponChargeEnergyDecrementing)
            {
                InvokeRepeating("WeaponEnergyDecrement", 1f, 0.5f);
                weaponChargeEnergyDecrementing = true;
            }      

            if (playerAbilitiesScript.energy <= 0)
            {
                weaponsAreCharged = false;
                weaponChargeEnergyDecrementing = false;
                CancelInvoke("WeaponEnergyDecrement");
                WeaponUnchargeStats();
            }
        }
    }

    private void WeaponEnergyDecrement()
    {
        playerAbilitiesScript.energy -= 1;
    }

    private void WeaponChargeStats()
    {
        gunData.fireRate *= gunData.fireRateFactor;

        gunData.magSize *= gunData.magSizeFactor;

        gunData.damage *= gunData.weaponChargeDMGFactor;
    }

    private void WeaponUnchargeStats()
    {
        gunData.fireRate /= gunData.fireRateFactor;

        gunData.magSize /= gunData.magSizeFactor;

        gunData.damage /= gunData.weaponChargeDMGFactor;
    }



    private IEnumerator HipfireDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }



    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.001f);
        if (canSwitchADS)
        {
            transform.localPosition = hipfirePos;
            transform.localPosition = Vector3.Lerp(curPos, hipfirePos, 0.25f * Time.deltaTime * 200);
            cam.fieldOfView = Mathf.Lerp(curFOV, defaultFOV, 0.25f * Time.deltaTime * 200);
        }
        else
        {
            StartCoroutine(HipfireDelay());
            transform.localPosition = Vector3.Lerp(curPos, hipfirePos, 0.25f * Time.deltaTime * 200);
            cam.fieldOfView = Mathf.Lerp(curFOV, defaultFOV, 0.25f * Time.deltaTime * 200);
        }
    }



    private IEnumerator ADSCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canSwitchADS = true;
    }



    public void SetGunRecoilData()
    {
    //Hipfire recoil
    recoilX = gunData.recoilX;
    recoilY = gunData.recoilY;
    recoilZ = gunData.recoilZ;

    adsRecoilX = gunData.adsRecoilX;
    adsRecoilY = gunData.adsRecoilY;
    adsRecoilZ = gunData.adsRecoilZ;

    //Settings
    snappiness = gunData.snappiness;
    returnSpeed = gunData.returnSpeed;
    }



    public void GunRecoil()
    {
        if (Input.GetMouseButton(1))
        {
            targetRotation += new Vector3(gunData.adsGunRecoilX, Random.Range(-gunData.adsGunRecoilY, gunData.adsGunRecoilY), Random.Range(-gunData.adsGunRecoilZ, gunData.adsGunRecoilZ));
            targetPosition += new Vector3(gunData.adsGunRecoilPosX, Random.Range(-gunData.adsGunRecoilPosY, gunData.adsGunRecoilPosY), gunData.adsGunRecoilPosZ);
        }
        else
        {
            targetRotation += new Vector3(gunData.gunRecoilX, Random.Range(-gunData.gunRecoilY, gunData.gunRecoilY), Random.Range(-gunData.gunRecoilZ, gunData.gunRecoilZ));
            targetPosition += new Vector3(gunData.gunRecoilPosX, Random.Range(-gunData.gunRecoilPosY, gunData.gunRecoilPosY), gunData.gunRecoilPosZ);
        }
        
    }



    private void OnDisable()
    {
        gunData.reloading = false;

    }



    private void OnEnable() 
    {
        animator.enabled = true;
        muzzleFlash.Stop();
        isSwitching = true;
        StartCoroutine(PulloutDelay());
    }



    private IEnumerator PulloutDelay()
    {
        yield return new WaitForSeconds(1.25f);
        isSwitching = false;
        animator.enabled = false;
    }


    
}

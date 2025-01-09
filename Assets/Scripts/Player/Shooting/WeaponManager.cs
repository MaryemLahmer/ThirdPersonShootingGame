using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")] [SerializeField] private float fireRate;
    float fireRateTimer;
    [SerializeField] private bool semiAuto;

    [Header("Bullet Properties")] [SerializeField]
    GameObject bullet;
    [SerializeField] private Transform bulletPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] private int bulletPerShot;
    public int damage = 20;
    private AimStateManager aim;
    private WeaponBloom bloom;

    
    [SerializeField] private AudioClip gunShot;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public WeaponAmmo ammo;
    ActionStateManager actions;
    private WeaponRecoil recoil;
    private WeaponClassManager weaponClass;

    [SerializeField] private Light muzzleFlashLight;
    ParticleSystem muzzleFlashParticles;
    float lightIntensity;
    [SerializeField] private float lightReturnSpeed = 20;


    void Start()
    {
        aim = GetComponentInParent<AimStateManager>();
        bloom = GetComponent<WeaponBloom>();
        fireRateTimer = fireRate;
        actions = GetComponentInParent<ActionStateManager>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponClassManager>();
            ammo = GetComponent<WeaponAmmo>();
            audioSource = GetComponent<AudioSource>();
            recoil = GetComponent<WeaponRecoil>();
            recoil.recoilFollowPos = weaponClass.recoilFollowPos;
        }
        weaponClass.SetCurrentWeapon(this);
       
    }

    void Update()
    {
        if (shouldFire() && aim.isAiming) Fire();
        // muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    bool shouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (ammo.currentAmmo == 0) return false;
        if (actions.currentState == actions.reload) return false;
        if (actions.currentState == actions.swap) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;
        bulletPos.LookAt(aim.aimPos);
        audioSource.PlayOneShot(gunShot);
        recoil.TriggerRecoil();
        // TriggerMuzzleFlash(); 
        ammo.currentAmmo--;
        for (int i = 0; i < bulletPerShot; i++)
        {
            GameObject bulletInstance = Instantiate(bullet, bulletPos.position , bulletPos.rotation);
            bulletInstance.SetActive(true);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
            bulletScript.weapon = this;
           Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            rb.AddForce(bulletPos.forward * bulletVelocity, ForceMode.Impulse);
            
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
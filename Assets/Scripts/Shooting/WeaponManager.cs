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

    [SerializeField] private Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] private int bulletPerShot;
    private AimStateManager aim;
    private WeaponAmmo ammo;
    private WeaponBloom bloom;
    [SerializeField] private AudioClip gunShot;
    AudioSource audioSource;
    private ActionStateManager actions;
    private WeaponRecoil recoil;

    [SerializeField]private Light muzzleFlashLight;
    ParticleSystem muzzleFlashParticles;
    float lightIntensity;
    [SerializeField] private float lightReturnSpeed = 20;


    void Start()
    {
        recoil = GetComponent<WeaponRecoil>();
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<AimStateManager>();
        ammo = GetComponent<WeaponAmmo>();
        bloom = GetComponent<WeaponBloom>();
        fireRateTimer = fireRate;
        actions = GetComponentInParent<ActionStateManager>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
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
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);
        barrelPos.localEulerAngles = bloom.BloomAngle(barrelPos);
        audioSource.PlayOneShot(gunShot);
        recoil.TriggerRecoil();
        // TriggerMuzzleFlash(); 
        ammo.currentAmmo--;
        for (int i = 0; i < bulletPerShot; i++)
        {
            GameObject bulletInstance = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
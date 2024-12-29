using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] private float fireRate;
    float fireRateTimer;
    [SerializeField] private bool semiAuto; 

    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet;
    [SerializeField] private Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] private int bulletPerShot;
    private AimStateManager aim;

    [SerializeField] private AudioClip gunShot;
    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<AimStateManager>();
        fireRateTimer = fireRate;
    }

    void Update()
    {
        if (shouldFire()) Fire(); 
        
    }

    bool shouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;

    }

    void Fire()
    {
        fireRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);
        audioSource.PlayOneShot(gunShot);
        for (int i = 0; i < bulletPerShot; i++)
        {
            GameObject bulletInstance = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
        
    }
}

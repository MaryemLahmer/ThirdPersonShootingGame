using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStateManager : MonoBehaviour
{
    [HideInInspector] public ActionBaseState currentState;

    public ReloadState reload = new ReloadState();
    public DefaultState defaultState = new DefaultState();

    public GameObject currentWeapon;
    [HideInInspector] public WeaponAmmo ammo;
    private AudioSource audioSource;
    [HideInInspector] public Animator anim;
    

    void Start()
    {
        SwitchState(defaultState);
        ammo = currentWeapon.GetComponent<WeaponAmmo>();
        audioSource = GetComponent<AudioSource>();
        
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(ActionBaseState State)
    {
        currentState = State;
        currentState.EnterState(this);
    }

    public void WeaponReloaded()
    {
        ammo.Reload();
        SwitchState(defaultState);
    }

    public void MagOut()
    {
        audioSource.PlayOneShot(ammo.magOutSound);
    }

    public void MagIn()
    {
        audioSource.PlayOneShot(ammo.magInSound);

    }

    public void ReloadSlide()
    {
        audioSource.PlayOneShot(ammo.releaseSlideSound);

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class ActionStateManager : MonoBehaviour
{
    [HideInInspector] public ActionBaseState currentState;

    public ReloadState reload = new ReloadState();
    public DefaultState defaultState = new DefaultState();
    public SwapState swap = new SwapState();
    
    [HideInInspector]public WeaponManager currentWeapon;
    [HideInInspector] public WeaponAmmo ammo;
    private AudioSource audioSource;
    [HideInInspector] public Animator anim;
    

    void Start()
    {
        SwitchState(defaultState);
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

    public void SetWeapon(WeaponManager weapon)
    {
        currentWeapon = weapon;
        audioSource = weapon.audioSource;
        ammo = weapon.ammo;
    }
}
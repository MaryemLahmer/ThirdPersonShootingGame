using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClassManager : MonoBehaviour
{
    private ActionStateManager actions;
    private AimStateManager aim;
    public Transform recoilFollowPos;
    public WeaponManager[] weapons;
    private int currentWeaponIndex;

    private void Awake()
    {
        currentWeaponIndex = 0;
        foreach ( var weapon  in weapons)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    public WeaponManager currentWeapon()
    {
        return weapons[currentWeaponIndex];
    }
    
    public void SetCurrentWeapon(WeaponManager weapon)
    {
        if (actions == null) actions = GetComponent<ActionStateManager>();
        actions.SetWeapon(weapon);
        weapon.gameObject.SetActive(true);
    }

    public void ChangeWeapon(float direction)
    {
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        if (direction < 0)
        {
            if (currentWeaponIndex == 0) currentWeaponIndex = weapons.Length - 1;
            else currentWeaponIndex--;
        }
        else
        {
            if (currentWeaponIndex == weapons.Length - 1) currentWeaponIndex = 0;
            else currentWeaponIndex++;
        }

        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    public void WeaponPutAway()
    {
        ChangeWeapon(actions.defaultState.scrollDirection);
    }

    public void WeaponPullOut()
    {

        actions.SwitchState(actions.defaultState);
    }
}
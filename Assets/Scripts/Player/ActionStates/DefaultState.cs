using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : ActionBaseState
{
    public float scrollDirection;
    public override void EnterState(ActionStateManager actions)
    {
        
    }

    public override void UpdateState(ActionStateManager actions)
    {
        if (Input.GetKeyDown(KeyCode.R) && CanReload(actions))
        {
            actions.SwitchState(actions.reload);
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            scrollDirection = Input.mouseScrollDelta.y;
            actions.SwitchState(actions.swap);
        }
    }

    bool CanReload(ActionStateManager action)
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize) return false;
        if (action.ammo.extraAmmo == 0) return false;
        return true;
    }
}
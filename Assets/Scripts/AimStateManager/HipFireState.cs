

using UnityEngine;

public class HipFireState : AimBaseState
{

    public override void EnterState(AimStateManager aim)
    {
        aim.animator.SetBool("Shooting", false);
        
    }

    public override void UpdateState(AimStateManager aim)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            aim.SwitchState(aim.Aim);
        }
        
    }
    
}

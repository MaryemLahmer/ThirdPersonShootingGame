

using UnityEngine;

// shooting with left mouse click
public class AimState : AimBaseState
{
    
    public override void EnterState(AimStateManager aim)
    {
        aim.animator.SetBool("Shooting", true);

    }

    public override void UpdateState(AimStateManager aim)
    {
        if(Input.GetKeyUp(KeyCode.Mouse0)) aim.SwitchState(aim.Hip);
      
    }
}

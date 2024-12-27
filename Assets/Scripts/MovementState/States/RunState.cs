using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Running", true);
        
    }

    public override void UpdateState(MovementStateManager movement, AimStateManager aim)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.Walk);
        if (Input.GetKeyUp(KeyCode.Space)) ExitState(movement, movement.Jump);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
        if (movement.vInput < 0) movement.currentMoveSpeed = movement.runBackSpeed;
        else movement.currentMoveSpeed = movement.runSpeed; 
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aim.isAiming = !aim.isAiming;
            aim.animator.SetBool("Aiming", aim.isAiming);
        }
    }
    
    
    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Running", false);
        movement.SwitchState(state);
    }
}

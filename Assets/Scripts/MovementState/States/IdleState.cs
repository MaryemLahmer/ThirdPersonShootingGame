using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    
    public override void EnterState(MovementStateManager movement)
    {
    }

    public override void UpdateState(MovementStateManager movement, AimStateManager aim)
    {
        if (movement.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            aim.animator.SetBool("Aiming", true);
            movement.SwitchState(movement.Crouch);
        }
        

        if (Input.GetKey(KeyCode.Space) && movement.IsGrounded())
        {
            movement.SwitchState(movement.Jump);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aim.isAiming = !aim.isAiming;
            aim.animator.SetBool("Aiming", aim.isAiming);
        }
        
    }
}
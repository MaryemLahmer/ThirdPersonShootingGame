using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Walking", true);
        
    }

    public override void UpdateState(MovementStateManager movement, AimStateManager aim)
    {
        if(Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        else if (Input.GetKeyDown(KeyCode.C)) ExitState(movement, movement.Crouch);
        else if (Input.GetKey(KeyCode.Space) && movement.IsGrounded()) ExitState(movement, movement.Jump);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.currentMoveSpeed = movement.walkBackSpeed;
        else movement.currentMoveSpeed = movement.walkSpeed; 
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aim.isAiming = !aim.isAiming;
            aim.animator.SetBool("Aiming", aim.isAiming);
            aim.animator.SetLayerWeight(1, aim.isAiming ? 1 : 0);

        }


    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}

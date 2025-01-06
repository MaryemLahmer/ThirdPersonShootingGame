using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        /*
        if(movement.previousState == movement.Idle) movement.anim.SetTrigger("IdleJump");
        else if (movement.previousState == movement.Run || movement.previousState == movement.Walk) movement.anim.SetTrigger("RunJump");
*/
        movement.anim.SetTrigger("IdleJump");
    }
    
    public override void UpdateState(MovementStateManager movement, AimStateManager aim)
    {
       // if (movement.jumped && movement.IsGrounded())
       if (movement.IsGrounded()) {
            movement.jumped = false;
            if(movement.hInput == 0 && movement.vInput ==0) movement.SwitchState(movement.Idle);
            else if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk); }
        
    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Jumping", false);

        movement.SwitchState(state);
    }
}

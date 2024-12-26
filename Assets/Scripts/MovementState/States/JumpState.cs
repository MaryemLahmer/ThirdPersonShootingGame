using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MovementBaseState
{
    
    
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Jumping", true);
        movement.velocity.y = Mathf.Sqrt(movement.jumpForce * -10f * movement.gravity);

    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
    }
    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Jumping", false);
        movement.SwitchState(state);
    }
}

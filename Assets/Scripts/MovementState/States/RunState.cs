using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Running", true);
        
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.Walk);
        if (Input.GetKeyUp(KeyCode.Space)) ExitState(movement, movement.Jump);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
    }
    
    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Running", false);
        movement.SwitchState(state);
    }
}

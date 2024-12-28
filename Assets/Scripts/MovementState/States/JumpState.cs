using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.StartCoroutine(HandleJumpTransition(movement));
       

    }

    private IEnumerator HandleJumpTransition(MovementStateManager movement)
    {
        movement.anim.SetBool("Jumping", true);
        yield return new WaitForSeconds(0.4f);
        movement.velocity.y = Mathf.Sqrt(movement.jumpForce * -8f * movement.gravity);

    }


    public override void UpdateState(MovementStateManager movement, AimStateManager aim)
    {
        if (movement.velocity.y <= 0 && movement.IsGrounded())
        {
            
            ExitState(movement, movement.Idle); 
        }
        else if (Input.GetKey(KeyCode.Space) && movement.IsGrounded())
        {
            ExitState(movement, movement.Jump); 
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ExitState(movement, movement.Run);
        }
    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Jumping", false);

        movement.SwitchState(state);
    }
}

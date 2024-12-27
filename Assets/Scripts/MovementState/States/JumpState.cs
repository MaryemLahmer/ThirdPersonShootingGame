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

        // Small delay to account for animation transition
        yield return new WaitForSeconds(0.3f);

        movement.velocity.y = Mathf.Sqrt(movement.jumpForce * -8f * movement.gravity);
        yield return new WaitForSeconds(0.3f);

    }


    public override void UpdateState(MovementStateManager movement)
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

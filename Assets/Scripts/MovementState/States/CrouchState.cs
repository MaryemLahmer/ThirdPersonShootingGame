using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.isCrouching = !movement.isCrouching;
        movement.anim.SetBool("Crouching", true);
        movement.currentMoveSpeed = movement.isCrouching ? movement.crouchSpeed : movement.walkSpeed;
    }

    public override void UpdateState(MovementStateManager movement, AimStateManager aim)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        if (Input.GetKeyDown(KeyCode.C)) ExitState(movement, movement.Idle);
        if (movement.vInput < 0)
            movement.currentMoveSpeed = movement.isCrouching ? movement.crouchBackSpeed : movement.walkSpeed;
        else
            movement.currentMoveSpeed = movement.isCrouching ? movement.crouchSpeed : movement.walkSpeed;

        movement.anim.SetFloat("Speed", Mathf.Abs(movement.vInput * movement.currentMoveSpeed));
        /*
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                aim.isAiming = !aim.isAiming;
                aim.animator.SetBool("Aiming", aim.isAiming);
            }
            */
    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Crouching", false);

        movement.SwitchState(state);

        movement.currentMoveSpeed = movement.walkSpeed;
    }
}
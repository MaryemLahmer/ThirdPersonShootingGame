using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    public float currentMoveSpeed = 50f;
    public float walkSpeed = 3f, walkBackSpeed = 2f;
    public float runSpeed = 7f, runBackSpeed = 5f;
    public float crouchSpeed = 2f, crouchBackSpeed = 1f;
    public float airSpeed = 1.5f;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hInput, vInput;
    [SerializeField] CharacterController controller;

    [SerializeField] private float groundYOffset;
    [SerializeField] LayerMask groundMask;
    private Vector3 shperePosition;

    [SerializeField] public float gravity;
    public Vector3 velocity;
    [SerializeField] public float jumpForce = 10f;
    [HideInInspector] public bool jumped;
    public MovementBaseState previousState;
    public MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();
    public JumpState Jump = new JumpState();

    public bool isCrouching;
    [HideInInspector] public Animator anim;
    [SerializeField] AimStateManager aim;

    void Start()
    {
        anim = GetComponent<Animator>();
        SwitchState(Idle);
        isCrouching = false;
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();
        Falling();
        anim.SetFloat("hInput", hInput);
        anim.SetFloat("vInput", vInput);
        currentState.UpdateState(this, aim);
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aim.isAiming = !aim.isAiming;
            aim.animator.SetBool("Aiming", aim.isAiming);
            aim.animator.SetLayerWeight(1, aim.isAiming ? 1 : 0);
        }
    }

    public void SwitchState(MovementBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    void GetDirectionAndMove()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        Vector3 airDir = Vector3.zero;
        if (!IsGrounded()) airDir = transform.forward * vInput + transform.right * hInput;
        else dir = (transform.forward * vInput + transform.right * hInput);
        controller.Move((dir.normalized * currentMoveSpeed + airDir.normalized * airSpeed) * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        shperePosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        bool grounded = Physics.CheckSphere(shperePosition, controller.radius - 0.05f, groundMask);
        return grounded;
    }


    void Gravity()
    {
        if (!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void Falling() => anim.SetBool("Falling", !IsGrounded());

    private void OnDrawGizmos()
    {
        shperePosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shperePosition, controller.radius - 0.05f);
    }

    public void JumpForce() => velocity.y += jumpForce;
    public void Jumped() => jumped = true;
}
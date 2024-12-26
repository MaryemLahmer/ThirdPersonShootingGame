using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    public float moveSpeed = 50f;
    [HideInInspector] public Vector3 dir;
    float hInput, vInput;
    [SerializeField] CharacterController controller;

    [SerializeField] private float groundYOffset;
    [SerializeField] LayerMask groundMask;
    private Vector3 shperePosition;

    [SerializeField] public float gravity = -19.81f;
    public Vector3 velocity;
    [SerializeField] public float jumpForce = 5f;

    private MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();
    public JumpState Jump = new JumpState();

    [HideInInspector] public Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        SwitchState(Idle);
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();
        anim.SetFloat("hInput", hInput);
        anim.SetFloat("vInput", vInput);
        currentState.UpdateState(this);
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
        dir = (transform.forward * vInput + transform.right * hInput);
        controller.Move(dir.normalized * moveSpeed * Time.deltaTime);
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

    private void OnDrawGizmos()
    {
        // Ensure this runs even in the editor
        shperePosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shperePosition, controller.radius - 0.05f);
    }
}
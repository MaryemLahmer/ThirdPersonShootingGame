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
    
    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    void Start()
    {
       // controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();
    }

    void GetDirectionAndMove()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        dir = (transform.forward * vInput + transform.right * hInput);
        controller.Move((dir  * moveSpeed) * Time.deltaTime);
        
    }

    bool IsGrounded()
    {
        shperePosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(shperePosition, controller.radius - 0.05f, groundMask)) return true;
        else return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y<0) velocity.y = -2;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shperePosition, controller.radius - 0.05f);
    }
}

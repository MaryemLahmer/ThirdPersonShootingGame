using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBloom : MonoBehaviour
{
    [SerializeField] private float defaultBloomAngle = 3f;
    [SerializeField] private float walkBloomMultiplier = 1.5f;
    [SerializeField] float crouchBloomMultiplier = 0.5f;
    [SerializeField] private float sprintBloomMultiplier = 2f;
    [SerializeField] float adsBloomMultiplier = 0.5f;
    
    private MovementStateManager movement;
    private AimStateManager aiming;

    private float currentBloom;
    void Start()
    {
        movement = GetComponentInParent<MovementStateManager>();
        aiming = GetComponentInParent<AimStateManager>();
        
        
    }

    public Vector3 BloomAngle(Transform barrelPos)
    {
        if(movement.currentState == movement.Idle) currentBloom = defaultBloomAngle;
        else if (movement.currentState == movement.Walk) currentBloom = walkBloomMultiplier * defaultBloomAngle;
        else if (movement.currentState == movement.Run) currentBloom = sprintBloomMultiplier * defaultBloomAngle;
        else if (movement.currentState == movement.Crouch)
        {
            if (movement.dir.magnitude == 0) currentBloom = crouchBloomMultiplier * defaultBloomAngle;
            else currentBloom = crouchBloomMultiplier * defaultBloomAngle * walkBloomMultiplier;
        }
        if (aiming.currentState == aiming.Aim)currentBloom *= adsBloomMultiplier;
        
        float randX = Random.Range(-currentBloom, currentBloom);
        float randY = Random.Range(-currentBloom, currentBloom);
        float randZ = Random.Range(-currentBloom, currentBloom);

        Vector3 randomRotation = new Vector3(randX, randY, randZ);
        return barrelPos.localEulerAngles + randomRotation;


    }
}
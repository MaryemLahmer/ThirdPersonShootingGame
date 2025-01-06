using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public Transform recoilFollowPos;
    [SerializeField] private float kickBackAmount = -1;
    [SerializeField] private float kickBackSpeed = 10, returnSpeed = 20;
    private float currentRecoilPosition, finalRecopilPosition;


    void Update()
    {
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, returnSpeed * Time.deltaTime);
        finalRecopilPosition = Mathf.Lerp(finalRecopilPosition, currentRecoilPosition, kickBackSpeed * Time.deltaTime);
        recoilFollowPos.localPosition = new Vector3(0, 0, finalRecopilPosition);
    }

    public void TriggerRecoil() => currentRecoilPosition += kickBackAmount;
}
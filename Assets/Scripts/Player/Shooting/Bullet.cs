using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    [HideInInspector] public WeaponManager weapon;

    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
        
    }

   

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(weapon.damage);
        }
        Destroy(this.gameObject);
    }
}
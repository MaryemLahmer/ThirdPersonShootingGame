using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 15f;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private float speed = 15f;

    private void Awake()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * (speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (explosionPrefab != null)
        {
            var explosion = Instantiate(explosionPrefab, other.contacts[0].point, Quaternion.identity);
            explosion.gameObject.SetActive(true);
            explosion.Play();
            Destroy(explosion.gameObject, 2f);
        }

        Destroy(gameObject);
    }
}
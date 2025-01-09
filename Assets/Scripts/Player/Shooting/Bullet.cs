using UnityEngine;
public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 15f;
    [SerializeField] private GameObject explosionEffectPrefab;
    private Rigidbody rb;
    [HideInInspector] public WeaponManager weapon;
    [SerializeField] private float speed = 1f;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, timeToDestroy);
    }
    private void FixedUpdate()
    {
        transform.position += transform.forward * (speed * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (explosionEffectPrefab != null) 
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            explosion.gameObject.SetActive(true);
            //explosion.Play();
            Destroy(explosion, 2f);
        }

        if (other.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(weapon.damage);
        }

        Destroy(this.gameObject);
    }
}
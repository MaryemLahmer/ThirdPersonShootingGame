using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private int initialHitPoints = 10;
    [SerializeField] private float moveCooldown = 3f;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private float angularSpeed = 360f;
    [SerializeField] private Transform target;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private ParticleSystem explosionPrefab;

    private int _hitPoints = 0;
    private float _moveTimer = 0f;
    private float _shootTimer = 0f;
    private NavMeshAgent _navMeshAgent;
    private UnityEvent<EnemyCharacter> _onDestroy = new();
    private RaycastHit[] _raycastHits = new RaycastHit[2];

    public float HitPointPercent => (float)_hitPoints / initialHitPoints;

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _hitPoints = initialHitPoints;
    }

    protected void OnEnable()
    {
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return null;
            _navMeshAgent.enabled = true;

            while (enabled)
            {
                _navMeshAgent.SetDestination(new Vector3(
                    Random.Range(-12f, 12f),
                    0f,
                    Random.Range(-12f, 12f)));

                do yield return null;
                while (_navMeshAgent.hasPath);

                do
                {
                    _shootTimer += Time.deltaTime;

                    var direction = (target.position - transform.position);
                    var lookRotation = Quaternion.LookRotation(direction.normalized);

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        lookRotation,
                        angularSpeed * Time.deltaTime);

                    bool hitWall = false;

                    // On peut lancer un raycast et voir si on hit quelque chose sur le chemin entre notre transform et la cible (exclue)
                    // Sinon on peut lancer un raycast jusqu'à la cible et récupérer les objets touchés. S'il y a plus d'un objet touché, ça veut dire qu'il y a un obstacle au milieu
                    // hitWall = Physics.Raycast(transform.position + Vector3.up, direction.normalized, direction.magnitude - 0.5f);
                    hitWall = Physics.RaycastNonAlloc(transform.position + Vector3.up, direction.normalized, _raycastHits, direction.magnitude) > 1;

                    if (_shootTimer >= shootCooldown && !hitWall)
                    {
                        _shootTimer = 0f;
                        Instantiate(bulletPrefab, transform.position + direction.normalized * 0.5f, lookRotation).gameObject.SetActive(true);
                    }

                    yield return null;
                    _moveTimer += Time.deltaTime;
                } while (_moveTimer < moveCooldown);

                _moveTimer = 0f;
                _shootTimer = 0f;
            }
        }
    }

    public void AddDestroyListener(UnityAction<EnemyCharacter> listener)
    {
        _onDestroy.AddListener(listener);
    }

    public void ApplyDamage(int value)
    {
        _hitPoints -= value;

        if (_hitPoints <= 0)
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.gameObject.SetActive(true);
            explosion.Play();
            Destroy(explosion.gameObject, explosion.main.duration);

            Destroy(gameObject);
            _onDestroy.Invoke(this);
        }
    }
}

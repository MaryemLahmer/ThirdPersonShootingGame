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
    [SerializeField] private GameObject bulletPrefab;
    private GameObject _bulletPrefabInstance; // Reference to keep a local copy

    [SerializeField] private ParticleSystem explosionPrefab;

    [SerializeField] float bulletVelocity;

    // Add boundaries for spawn area
    [SerializeField] private float minX = -12f;
    [SerializeField] private float maxX = 12f;
    [SerializeField] private float minZ = -502f;
    [SerializeField] private float maxZ = -498f;

    private int _hitPoints = 0;
    private float _moveTimer = 0f;
    private float _shootTimer = 0f;
    private NavMeshAgent _navMeshAgent;
    private UnityEvent<EnemyCharacter> _onDestroy = new();
    private RaycastHit[] _raycastHits = new RaycastHit[2];

    public float HitPointPercent => (float)_hitPoints / initialHitPoints;

    private void Start()
    {
        _bulletPrefabInstance = Instantiate(bulletPrefab);
        _bulletPrefabInstance.SetActive(true);
        DontDestroyOnLoad(_bulletPrefabInstance);
    }

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _shootTimer = shootCooldown; // Start ready to shoot
        _hitPoints = initialHitPoints;
    }

    protected void OnEnable()
    {
        StartCoroutine(InitializeAIRoutine());
    }

    private IEnumerator InitializeAIRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            _navMeshAgent.enabled = true;
            StartCoroutine(AIRoutine());
        }
        else
        {
            Debug.LogError("Enemy spawned outside of NavMesh!");
            gameObject.SetActive(false);
        }
    }

    private IEnumerator AIRoutine()
    {
        while (true)
        {
            Vector3 randomDestination = GetRandomNavMeshPoint();
            if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.SetDestination(randomDestination);

                yield return new WaitUntil(() => _navMeshAgent.hasPath);

                // if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    do
                    {
                        _shootTimer += Time.deltaTime;

                        var direction = (target.position - transform.position).normalized;
                        var lookRotation = Quaternion.LookRotation(direction);

                        transform.rotation = Quaternion.RotateTowards(
                            transform.rotation,
                            lookRotation,
                            angularSpeed * Time.deltaTime);

                        bool hitWall = Physics.RaycastNonAlloc(
                            transform.position + Vector3.up,
                            direction,
                            _raycastHits,
                            direction.magnitude) > 1;

                        if (_shootTimer >= shootCooldown)
                        {
                            _shootTimer = 0f;

                            Vector3 spawnPos = transform.position + transform.forward * 1.5f;

                            GameObject bulletInstance =
                                Instantiate(_bulletPrefabInstance, spawnPos, transform.rotation);
                            bulletInstance.SetActive(true);

                         
                        }

                        yield return null;
                        _moveTimer += Time.deltaTime;
                    } while (_moveTimer < moveCooldown);
                }
            }

            _moveTimer = 0f;
            _shootTimer = 0f;
            yield return null;
        }
    }

    /*
    private void OnDestroy()
    {
        // Clean up our template instance when the enemy is destroyed
        if (_bulletPrefabInstance != null)
        {
            Destroy(_bulletPrefabInstance);
        }
    }
*/
    private Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomPoint;
        NavMeshHit hit;
        int maxAttempts = 30;
        int attempts = 0;

        do
        {
            randomPoint = new Vector3(
                Random.Range(minX, maxX),
                71f,
                Random.Range(minZ, maxZ)
            );
            attempts++;
        } while (!NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas)
                 && attempts < maxAttempts);

        return hit.position;
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
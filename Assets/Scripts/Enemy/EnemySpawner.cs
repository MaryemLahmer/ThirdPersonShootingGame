using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float maxEnemyCount = 10f;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private EnemyCharacter enemyPrefab;
    [SerializeField] private float spawnHeight = 70.1f; // Slightly above plane height

    [Header("Spawn Area")] [SerializeField]
    private Vector2 xRange = new Vector2(-22f, 22f);

    [SerializeField] private Vector2 zRange = new Vector2(-502f, -498f);


    private int _enemyKilledCount = 0;
    private float _spawnTimer = 0f;
    private List<EnemyCharacter> _enemyCharacters = new();

    public int EnemyKilledCount => _enemyKilledCount;

    private void OnDrawGizmos()
    {
        Vector3 center = new Vector3(
            (xRange.x + xRange.y) / 2f,
            spawnHeight,
            (zRange.x + zRange.y) / 2f
        );
        Vector3 size = new Vector3(
            xRange.y - xRange.x,
            0.1f,
            zRange.y - zRange.x
        );
        Gizmos.DrawCube(center, size);
    }

    private IEnumerator Start()
    {
        while (true)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer > spawnInterval)
            {
                _spawnTimer = 0f;

                if (_enemyCharacters.Count >= maxEnemyCount) break;

                SpawnEnemy();
            }

            yield return null;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 randomPos = GetRandomSpawnPosition();
        NavMeshHit hit;

        // Try to find a valid NavMesh position
        if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
        {
            Vector3 spawnPos = new Vector3(hit.position.x, spawnHeight, hit.position.z);
            var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
            enemy.gameObject.SetActive(true);
            _enemyCharacters.Add(enemy);
            enemy.AddDestroyListener(OnEnemyCharacterDestroyed);
        }
        else
        {
            Debug.LogWarning($"Failed to find valid NavMesh position near {randomPos}. Retrying next interval.");
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(xRange.x, xRange.y),
            spawnHeight,
            Random.Range(zRange.x, zRange.y)
        );
    }

    private void OnEnemyCharacterDestroyed(EnemyCharacter enemy)
    {
        _enemyCharacters.Remove(enemy);
        _enemyKilledCount += 1;
    }
}
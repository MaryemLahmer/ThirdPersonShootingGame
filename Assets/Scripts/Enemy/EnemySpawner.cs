using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float maxEnemyCount = 10f;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private EnemyCharacter enemyPrefab;

    private int _enemyKilledCount = 0;
    private float _spawnTimer = 0f;
    private List<EnemyCharacter> _enemyCharacters = new();

    public int EnemyKilledCount => _enemyKilledCount;

    private IEnumerator Start()
    {
        while(true)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer > spawnInterval)
            {
                _spawnTimer = 0f;

                if (_enemyCharacters.Count >= 10) break;

                var enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-12f, 12f), 0, Random.Range(-12f, 12f)), Quaternion.identity, transform);
                enemy.gameObject.SetActive(true);
                _enemyCharacters.Add(enemy);
                enemy.AddDestroyListener(OnEnemyCharacterDestroyed);
            }

            yield return null;
        }
    }

    private void OnEnemyCharacterDestroyed(EnemyCharacter enemy)
    {
        _enemyCharacters.Remove(enemy);
        _enemyKilledCount += 1;
    }
}
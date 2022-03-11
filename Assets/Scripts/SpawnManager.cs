using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _spawnWaitTimeLevel = 1f;
    [SerializeField]
    private bool _stopSpawning = true;

    [SerializeField]
    private GameObject[] _powerUpPrefabs;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    public void StopSpawning()
    {
        _stopSpawning = true;
    }

    public void StartSpawning()
    {
        _stopSpawning = false;
        StartCoroutine(SpwanEnemyRoutine());
        StartCoroutine(SpwanTripleLaserPowerUpRoutine());
    }

    IEnumerator SpwanEnemyRoutine()
    {
        while (!_stopSpawning)
        {
            float randomSeconds = Random.Range(0.85f, 1.35f);
            yield return new WaitForSeconds(randomSeconds / _spawnWaitTimeLevel);

            float randomX = Random.Range(-7.6f, 7.6f);
            GameObject enemy = Instantiate(_enemyPrefab, new Vector3(randomX, 9, 0), Quaternion.identity);
            enemy.transform.parent = _enemyContainer.transform;
        }
    }

    IEnumerator SpwanTripleLaserPowerUpRoutine()
    {
        while (!_stopSpawning)
        {
            float randomSeconds = Random.Range(2f, 6f);
            yield return new WaitForSeconds(randomSeconds);

            float randomX = Random.Range(-8f, 8f);
            int randomPowerUpId = Random.Range(0, 3);
            Instantiate(_powerUpPrefabs[randomPowerUpId], new Vector3(randomX, 9, 0), Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public Coroutine spawnEnemy;
    public Coroutine spawnPowerup;

    [SerializeField]
    private GameObject _enemyShipPrefab;

    [SerializeField]
    private GameObject[] _powerups;

    private GameManager _gameManager;

    void Start() {
        _gameManager = GetComponentInParent<GameManager>();
    }

    public IEnumerator SpawnEnemyRoutine() {
        while(!_gameManager.gameOver) {
            float random = Random.Range(_gameManager.MinX + 1, _gameManager.MaxX - 1);

            Instantiate(_enemyShipPrefab, new Vector3(random, _gameManager.MaxY + 2, 0), Quaternion.identity);

            yield return new WaitForSeconds(1.0f);
        }
    }

    public IEnumerator SpawnPowerupRoutine() {
        while(!_gameManager.gameOver) {
            int randomPowerup = Random.Range(0, 3);
            float randomPositionX = Random.Range(_gameManager.MinX + 1, _gameManager.MaxX - 1);

            Instantiate(_powerups[randomPowerup], new Vector3(randomPositionX, _gameManager.MaxY + 2, 0), Quaternion.identity);

            yield return new WaitForSeconds(5.0f);
        }
    }

    public void StartSpawning() {
        spawnEnemy = StartCoroutine(SpawnEnemyRoutine());
        spawnPowerup = StartCoroutine(SpawnPowerupRoutine());
    }

    public void StopSpawning() {
        if (spawnEnemy != null && spawnPowerup != null) {
            StopCoroutine(spawnEnemy);
            StopCoroutine(spawnPowerup);
        }
    }
}

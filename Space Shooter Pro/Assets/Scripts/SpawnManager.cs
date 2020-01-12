using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _spawnWait = 5f;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUps;

    private bool _stopSpawning = false;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Stops spawning upon player death;
    /// </summary>
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    /// <summary>
    /// The enemy spawning routine
    /// </summary>
    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_spawnWait);
        }
    }

    /// <summary>
    /// Spawn Routine for powerups
    /// </summary>
    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    #endregion
}

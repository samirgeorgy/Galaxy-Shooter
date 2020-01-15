using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Private Variables

    static private SpawnManager _instance;

    [SerializeField] private float _spawnWait = 5f;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUps;

    private bool _stopSpawning = false;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets an instance of the SpawnManager
    /// </summary>
    static public SpawnManager Instance
    {
        get { return _instance; }
    }

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Starts spawning waves of enemies
    /// </summary>
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
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
        yield return new WaitForSeconds(3.5f);

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
        yield return new WaitForSeconds(3.5f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 6);
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    #endregion
}

using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region public Enum

    /// <summary>
    /// Spawn States
    /// </summary>
    public enum SpawnState
    {
        Spawning,
        Waiting,
        Counting
    }

    #endregion

    #region Private Variables

    static private SpawnManager _instance;

    [Header("Enemy Spawn Settings")]
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private int[] _enemyRarityTable;
    [SerializeField] private GameObject _enemyContainer;

    [Header("Power Ups and Rarities Settings")]
    [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private int[] _powerUpRarityTable;

    [Header("Wave Settings")]
    [SerializeField] private float _timeBetweenWaves = 5.0f;

    private bool _stopSpawning = true;
    private int _totalEnemyWeight;
    private int _totalPowerUpWeight;
    private float _waveCountDown;
    private int _waveIndex = 1;
    private float _enemySearchCountDown = 1.0f;
    private bool _bossFightInstantiated = false;
    private int _bossFightRate = 2;
    private SpawnState _state = SpawnState.Counting;

    private int _waveMultiplier = 2;
    private float _waveSpawnRate = 5;

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

        for (int i = 0; i < _enemyRarityTable.Length; i++)
            _totalEnemyWeight += _enemyRarityTable[i];

        for (int i = 0; i < _powerUpRarityTable.Length; i++)
            _totalPowerUpWeight += _powerUpRarityTable[i];

        _waveCountDown = _timeBetweenWaves;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_stopSpawning == false)
        {
            if (_state == SpawnState.Waiting)
            {
                if (EnemyIsAlive() == false)
                {
                    StartNewWave();
                }
                else
                    return;
            }

            if (_waveCountDown <= 0)
            {
                if (_state != SpawnState.Spawning)
                    StartCoroutine(SpawnEnemyWaveRoutine());

            }
            else
            {
                _waveCountDown -= Time.deltaTime;
            }
        }
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Starts spawning waves of enemies
    /// </summary>
    public void StartSpawning()
    {
        _stopSpawning = false;
        UIManager.Instance.ShowWaveNumber(_waveIndex);
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
    /// Starts a new wave
    /// </summary>
    private void StartNewWave()
    {
        _state = SpawnState.Counting;
        _waveCountDown = _timeBetweenWaves;

        if (_waveSpawnRate > 0.5f)
            _waveSpawnRate -= 0.5f;

        _waveIndex++;
        UIManager.Instance.ShowWaveNumber(_waveIndex);
    }

    /// <summary>
    /// Checks whether there are any alive enemies in the current wave
    /// </summary>
    /// <returns>True if there are alive enemies and false otherwise</returns>
    private bool EnemyIsAlive()
    {
        _enemySearchCountDown -= Time.deltaTime;

        if (_enemySearchCountDown <= 0)
        {
            _enemySearchCountDown = 1.0f;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                if ((_waveIndex % _bossFightRate == 0) && (_bossFightInstantiated == false))
                {
                    _bossFightInstantiated = true;
                    SpawnBoss();
                }
                else
                {
                    _bossFightInstantiated = false;
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Selects a random Power Up based on the rarity table
    /// </summary>
    /// <returns>The index of the selected powerup</returns>
    private int SelectWeightedPowerUp()
    {
        int random = Random.Range(0, _totalPowerUpWeight);

        for (int i = 0; i < _powerUpRarityTable.Length; i++)
        {
            if (random <= _powerUpRarityTable[i])
                return i;
            else
                random -= _powerUpRarityTable[i];
        }

        return -1;
    }

    /// <summary>
    /// Selects a random Enemy based on the rarity table
    /// </summary>
    /// <returns>The index of the selected enemy</returns>
    private int SelectWeightedEnemy()
    {
        int random = Random.Range(0, _totalEnemyWeight);

        for (int i = 0; i < _enemyRarityTable.Length; i++)
        {
            if (random <= _enemyRarityTable[i])
                return i;
            else
                random -= _enemyRarityTable[i];
        }

        return -1;
    }

    /// <summary>
    /// Spawns the boss
    /// </summary>
    private void SpawnBoss()
    {
        Vector3 posToSpawn = new Vector3(0, 10.5f, 0);
        GameObject newEnemy = Instantiate(_bossPrefab, posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    /// <summary>
    /// The enemy wave spawning routine
    /// </summary>
    IEnumerator SpawnEnemyWaveRoutine()
    {
        _state = SpawnState.Spawning;

        int numberOfEnemies = _waveIndex * _waveMultiplier;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int enemyIndex = SelectWeightedEnemy();

            if (enemyIndex != -1)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab[enemyIndex], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }

            yield return new WaitForSeconds(_waveSpawnRate);
        }

        _state = SpawnState.Waiting;
        yield break;
    }

    /// <summary>
    /// Spawn Routine for powerups
    /// </summary>
    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.5f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-8f, 8f), 7, 0);
            int powerupIndex = SelectWeightedPowerUp();

            if (powerupIndex != -1)
                Instantiate(_powerUps[powerupIndex], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }


    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
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

    List<Tuple<int, int>> _rarityValues;

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

        _rarityValues = new List<Tuple<int, int>>();
        _rarityValues.Add(new Tuple<int, int>(0, 18));
        _rarityValues.Add(new Tuple<int, int>(19, 36));
        _rarityValues.Add(new Tuple<int, int>(37, 54));
        _rarityValues.Add(new Tuple<int, int>(55, 72));
        _rarityValues.Add(new Tuple<int, int>(73, 90));
        _rarityValues.Add(new Tuple<int, int>(91, 100));
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
            Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-8f, 8f), 7, 0);
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
            Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = UnityEngine.Random.Range(0, 101);

            for (int i = 0; i < _rarityValues.Count; i++)
            {
                if ((randomPowerUp >= _rarityValues[i].Item1) && (randomPowerUp <= _rarityValues[i].Item2))
                {
                    Instantiate(_powerUps[i], posToSpawn, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(3, 8));
        }
    }

    #endregion
}

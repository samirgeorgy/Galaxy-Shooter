using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    #region Private Variables

    [SerializeField]
    private GameObject _enemyShipPrefab;

    [SerializeField]
    private GameObject[] _powerUps;

    private GameManager _gameManager;

    #endregion

    #region API Methods

    // Use this for initialization
    void Start () {

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUps());
    }

    /// <summary>
    /// The function that spawns the enemy
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemy()
    {
        while (_gameManager.isGameRunning)
        {
            Instantiate(_enemyShipPrefab, new Vector3(Random.Range(-7.9f, 7.9f), 6.2f, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

    /// <summary>
    /// The function that spawns the power ups.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnPowerUps()
    {
        while (_gameManager.isGameRunning)
        {
            int powerUpIndex = Random.Range(0, 3);
            Instantiate(_powerUps[powerUpIndex], new Vector3(Random.Range(-7.9f, 7.9f), 6.2f, 0), Quaternion.identity);
            yield return new WaitForSeconds(10.0f);
        }
    }

    #endregion
}

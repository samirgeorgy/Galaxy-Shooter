using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool isGameRunning = false;

    [SerializeField]
    private GameObject playerCharacterPrefab;

    private UIManager _uIManager;

    [SerializeField]
    private SpawnManager _spawnManager;

    private void Start()
    {
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uIManager.ShowTitleScreen();
    }

    // Update is called once per frame
    void Update () {

        if (!isGameRunning)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(playerCharacterPrefab, new Vector3(0, -3, 0), Quaternion.identity);
                isGameRunning = true;

                if (_uIManager != null)
                    _uIManager.HideTitleScreen();

                _spawnManager.StartSpawning();
            }
        }
    }

    /// <summary>
    /// Updates the game status to whether running or not.
    /// </summary>
    /// <param name="gameStatus">The game status to whether it is running or not</param>
    public void UpdateGameStatus(bool gameStatus)
    {
        isGameRunning = gameStatus;
    }
}

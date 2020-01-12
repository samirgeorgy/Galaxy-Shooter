using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Private Variables

    static private GameManager _instance;

    bool _isGameOver = false;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets an instance of the game manager
    /// </summary>
    static public GameManager Instance
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Marks the game as over
    /// </summary>
    public void GameOver()
    {
        _isGameOver = true;
    }

    #endregion
}

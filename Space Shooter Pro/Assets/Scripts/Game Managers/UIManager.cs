using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _ammoText;
    [SerializeField] private Text _thrustersLevel;
    [SerializeField] private Text _bossHealthText;
    [SerializeField] private Image _livesImage; 
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _waveNumberText;
    [SerializeField] private Text _missileAmmoText;
    [SerializeField] private Text _messageText;

    static private UIManager _instance;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets an instance of the UIManager
    /// </summary>
    static public UIManager Instance
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

        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Updates the score value in the UI
    /// </summary>
    /// <param name="score">The score value to be updated/</param>
    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Updates the ammo count in the UI
    /// </summary>
    /// <param name="ammo">The ammo count to be updated;</param>
    public void UpdateAmmoText(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo.ToString() + "/15";
    }

    /// <summary>
    /// Updates the lives display in the UI
    /// </summary>
    /// <param name="currentLives">The number of lives the player has</param>
    public void UpdateLives(int currentLives)
    {
        if ((currentLives < _livesSprites.Length) && (currentLives >= 0))
            _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    /// <summary>
    /// Updates the boss lives on the UI
    /// </summary>
    /// <param name="lives">The lives to be updated</param>
    public void UpdateBossLives(int lives)
    {
        _bossHealthText.text = "Boss Health: " + lives.ToString() + "%";
    }

    /// <summary>
    /// Enables the boss heath Text
    /// </summary>
    public void EnableBossLivesText()
    {
        _bossHealthText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables the boss heath Text
    /// </summary>
    public void DisableBossLivesText()
    {
        _bossHealthText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the player's thursters level in the UI
    /// </summary>
    /// <param name="level">The leve to be updated</param>
    public void UpdateThurstersLevel(string level)
    {
        _thrustersLevel.text = "Thrusters: " + level;
    }

    /// <summary>
    /// Displays the wave number in the UI
    /// </summary>
    /// <param name="number">The wave number to be displayed</param>
    public void ShowWaveNumber(int number)
    {
        _waveNumberText.text = "Wave " + number.ToString();
        _waveNumberText.gameObject.SetActive(true);
        StartCoroutine(DeactivateWaveNumberText());
    }

    /// <summary>
    /// What happens in the UI when the game is over
    /// </summary>
    private void GameOverSequence()
    {
        GameManager.Instance.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    /// <summary>
    /// Updates the missile ammo count in the UI
    /// </summary>
    /// <param name="count">The count to be updated</param>
    public void UpdateMissileAmmoText(int count)
    {
        _missileAmmoText.text = "Missiles: " + count.ToString() + "/3";
    }

    /// <summary>
    /// Enables the message text
    /// </summary>
    public void EnableMessageText()
    {
        _messageText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables the text message
    /// </summary>
    public void DisableMessageText()
    {
        _messageText.gameObject.SetActive(false);
    }

    /// <summary>
    /// A corrutine to simulate the flicker effect of the game over text
    /// </summary>
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Deactivates the wave text in the UI
    /// </summary>
    /// <returns></returns>
    IEnumerator DeactivateWaveNumberText()
    {
        yield return new WaitForSeconds(2.0f);
        _waveNumberText.gameObject.SetActive(false);
    }

    #endregion
}

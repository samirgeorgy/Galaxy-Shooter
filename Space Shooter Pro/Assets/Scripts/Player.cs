using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score = 0;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private AudioClip _laserSFX;

    private AudioSource _audioSource;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private float _canFire = -1f;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
            Debug.LogError("Audio Source on the player is null!");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && (Time.time > _canFire))
            FireLaser();
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Moves the player
    /// </summary>
    private void CalculateMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalMovement, verticalMovement, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        else if (transform.position.x < -11.3f)
            transform.position = new Vector3(11.3f, transform.position.y, 0);
    }

    /// <summary>
    /// Fires the laser
    /// </summary>
    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.PlayOneShot(_laserSFX);
    }

    /// <summary>
    /// Adds score to the player
    /// </summary>
    /// <param name="score">The score value to be added</param>
    public void AddScore(int score)
    {
        _score += score;
        UIManager.Instance.UpdateScoreText(_score);
    }

    /// <summary>
    /// Damages the player
    /// </summary>
    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        UIManager.Instance.UpdateLives(_lives);

        if (_lives == 2)
            _leftEngine.SetActive(true);
        else if (_lives == 1)
            _rightEngine.SetActive(true);

        if (_lives < 1)
        {
            SpawnManager.Instance.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Enables the triple shot power up
    /// </summary>
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    /// <summary>
    /// Enables the speed boost power up
    /// </summary>
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    /// <summary>
    /// Activates the shield
    /// </summary>
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    /// <summary>
    /// A routine to stop the triple shot power up
    /// </summary>
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isTripleShotActive = false;
    }

    /// <summary>
    /// A routine to stop the speed power up
    /// </summary>
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    #endregion
}

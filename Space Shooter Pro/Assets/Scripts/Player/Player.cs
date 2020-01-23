using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _SpeedThruster = 2;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _shieldEndurance = 3;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _maxAmmoCount = 15;
    [SerializeField] private int _missileAmmoCount = 0;
    [SerializeField] private int _maxMissileAmmo = 3;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _mayhemPrefab;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _laserSFX;
    [SerializeField] private AudioClip _explosionSFX;
    [SerializeField] private AudioClip _missileSFX;
    [SerializeField] private Animator _playerAnimation;
    [SerializeField] private Animator _cameraAnimator;
    [SerializeField] private SpriteRenderer _shieldMaterial;

    private AudioSource _audioSource;

    private int _currentAmmoCount;
    private int _thrustersLevel = 100;
    private bool _canUseThrusters = true;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _isMayhemActive = false;
    private float _canFire = -1f;
    private bool _isPlayerDead = false;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        _currentAmmoCount = _maxAmmoCount;
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

        if (Input.GetKeyDown(KeyCode.F) && (_missileAmmoCount != 0))
            FireMissile();

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

        if (Input.GetKey(KeyCode.LeftShift) && (_canUseThrusters == true))
        {
            float newSpeed = _speed + _SpeedThruster;
            transform.Translate(direction * newSpeed * Time.deltaTime);
            _thrustersLevel -= 1;
            UIManager.Instance.UpdateThurstersLevel(_thrustersLevel.ToString() + "%");

            if (_thrustersLevel == 0)
            {
                _canUseThrusters = false;
                StartCoroutine(RechargeThursters());
            }
        }
        else
            transform.Translate(direction * _speed * Time.deltaTime);

        _playerAnimation.SetFloat("Horizontal", horizontalMovement);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 3.8f), 0);

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
            _audioSource.PlayOneShot(_laserSFX);
        }
        if (_isMayhemActive == true)
        {
            Instantiate(_mayhemPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserSFX);
        }
        else
        {
            if (_currentAmmoCount > 0)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                _currentAmmoCount--;
                UIManager.Instance.UpdateAmmoText(_currentAmmoCount);
                _audioSource.PlayOneShot(_laserSFX);
            }
        }
    }

    /// <summary>
    /// Fires the missile
    /// </summary>
    private void FireMissile()
    {
        Instantiate(_missilePrefab, transform.position, Quaternion.identity);
        _missileAmmoCount--;
        UIManager.Instance.UpdateMissileAmmoText(_missileAmmoCount);
        _audioSource.PlayOneShot(_missileSFX);

        if (_missileAmmoCount <= 0)
            UIManager.Instance.DisableMessageText();
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
            _shieldEndurance--;

            if (_shieldEndurance == 2)
            {
                _shieldMaterial.material.color = Color.yellow;
                return;
            }
            else if (_shieldEndurance == 1)
            {
                _shieldMaterial.material.color = Color.red;
                return;
            }
            else if (_shieldEndurance == 0)
            {
                _isShieldActive = false;
                _shieldVisualizer.SetActive(false);
                _shieldMaterial.material.color = Color.white;
                return;
            }
        }

        _lives--;
        _cameraAnimator.SetTrigger("ShakeCamera");
        UIManager.Instance.UpdateLives(_lives);

        if (_lives == 2)
            _leftEngine.SetActive(true);
        else if (_lives == 1)
            _rightEngine.SetActive(true);

        if (_lives < 1)
        {
            _isPlayerDead = true;
            SpawnManager.Instance.OnPlayerDeath();
            this.gameObject.SetActive(false);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_explosionSFX);
            Destroy(this.gameObject, 3.0f);
        }
    }

    /// <summary>
    /// Checks whether the player is dead or not
    /// </summary>
    /// <returns>True of player is dead and false otherwise</returns>
    public bool IsPlayerDead()
    {
        return _isPlayerDead;
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
        _shieldEndurance = 3;
        _shieldMaterial.material.color = Color.white;
        _shieldVisualizer.SetActive(true);
    }

    /// <summary>
    /// Reloads the player's ammo
    /// </summary>
    public void ReloadAmmo()
    {
        _currentAmmoCount = _maxAmmoCount;
        UIManager.Instance.UpdateAmmoText(_currentAmmoCount);
    }

    /// <summary>
    /// Adds 1 health point to the player
    /// </summary>
    public void AddHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            UIManager.Instance.UpdateLives(_lives);

            if (_lives > 2)
                _leftEngine.SetActive(false);
            else if (_lives > 1)
                _rightEngine.SetActive(false);
        }
    }

    /// <summary>
    /// Enables the Mayhem power up
    /// </summary>
    public void MayhemActive()
    {
        _isMayhemActive = true;
        StartCoroutine(MayhemPowerDownRoutine());
    }

    /// <summary>
    /// Crumbles the engine
    /// </summary>
    public void EngineCrumble()
    {
        _speed /= 2;
        StartCoroutine(FixEnginesRoutine());

    }

    /// <summary>
    /// Adds One Missile Ammo
    /// </summary>
    public void AddMissileAmmo()
    {
        if (_missileAmmoCount < _maxMissileAmmo)
        {
            _missileAmmoCount++;
            UIManager.Instance.UpdateMissileAmmoText(_missileAmmoCount);
            UIManager.Instance.EnableMessageText();
        }
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

    /// <summary>
    /// A routine to stop the Mayhem power up
    /// </summary>
    IEnumerator MayhemPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isMayhemActive = false;
    }

    /// <summary>
    /// A routine to recharge the player's thrusters
    /// </summary>
    IEnumerator RechargeThursters()
    {
        yield return new WaitForSeconds(5.0f);

        while (_thrustersLevel != 100)
        {
            _thrustersLevel += 1;
            UIManager.Instance.UpdateThurstersLevel(_thrustersLevel.ToString() + "%");
        }

        _canUseThrusters = true;
    }

    /// <summary>
    /// Fixes the engine thrusters
    /// </summary>
    IEnumerator FixEnginesRoutine()
    {
        UIManager.Instance.UpdateThurstersLevel("Engine Error! Attempting Restart!");
        _thrustersLevel = 0;

        yield return new WaitForSeconds(4);

        UIManager.Instance.UpdateThurstersLevel("Engine Restart Successful! Power Regained...");

        yield return new WaitForSeconds(1);

        _speed *= 2;
        _thrustersLevel = 100;
        UIManager.Instance.UpdateThurstersLevel(_thrustersLevel.ToString() + "%");
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls to the player object
/// </summary>
public class Player : MonoBehaviour {

    #region Public Variables

    //Checks if we can do a triple shot or not
    public bool canTripleShot = false;

    //The player default life count
    public int playerLives = 3;

    #endregion

    #region Private Variables

    //The speed of the player
    [SerializeField]
    private float _speed = 5.0f;

    //The laser shot
    [SerializeField]
    private GameObject _laserPrefab;

    //The laser Triple Shot
    [SerializeField]
    private GameObject _laserTripleShotPrefab;

    //The player explosion animation
    [SerializeField]
    private GameObject _playerExplosionPrefab;

    //The right Engine Failure
    [SerializeField]
    private GameObject _rightEngineFailure;

    //The left Engine Failure
    [SerializeField]
    private GameObject _leftEngineFailure;

    //The player Shield
    [SerializeField]
    private GameObject _playerShield;

    //Fire rate of the player
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = 0.0f;

    //Shows whether the shield is on or not
    [SerializeField]
    private bool _isShieldOn = false;

    //The UI Manager
    UIManager _uiManager;

    //The Game Manager
    GameManager _gameManager;

    //The audio source of the player
    AudioSource _audioSource;

    #endregion

    #region API Methods

    // Use this for initialization
    void Start () {

        //Assign the default position to the player
        transform.position = new Vector3(0, -3, 0);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_uiManager != null)
            _uiManager.UpdateLives(playerLives);

        _audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        //Move the player when the user strokes an input on the keyboard
        MovePlayer();

        //Shoot the laser
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            ShootLaser();
    }

    #endregion

    #region Supporting Methods

    /// <summary>
    /// This function moves the player
    /// </summary>
    private void MovePlayer ()
    {
        //Capturing user input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Translating the player on the X-axis
        transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);

        //Translating the player on the Y-axis
        transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);

        //Constraning the player movement on the Y-axis
        if (transform.position.y > 0)
            transform.position = new Vector3(transform.position.x, 0, 0);
        else if (transform.position.y < -4.2f)
            transform.position = new Vector3(transform.position.x, -4.2f, 0);

        //Constraning the player movement on the X-axis
        if (transform.position.x > 8.2f)
            transform.position = new Vector3(8.2f, transform.position.y, 0);
        else if (transform.position.x < -8.2f)
            transform.position = new Vector3(-8.2f, transform.position.y, 0);
    }

    /// <summary>
    /// This function spawns the laser
    /// </summary>
    private void ShootLaser ()
    {
        //Here we control the fire rate of the laser.
        //The time of each laser beam has to be seperated by 0.5 seconds off the last one fired.
        if (Time.time > _canFire)
        {
            _audioSource.Play();

            //If the player picked the power up, we do the triple shot.
            if (canTripleShot)
            {
                Instantiate(_laserTripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            _canFire = Time.time + _fireRate;
        }
    }

    /// <summary>
    /// This function makes damage to the player and destroys it if the lives reach zero.
    /// </summary>
    public void DamagePlayer()
    {
        //If the shield is not on, we deduce on life, otherwise we destroy the shield.
        if (_isShieldOn)
        {
            _isShieldOn = false;
            _playerShield.SetActive(false);
            return;
        }

        playerLives--;
        _uiManager.UpdateLives(playerLives);

        //Displaying the engine failure based on the damage
        switch(playerLives)
        {
            case 2:
                _rightEngineFailure.SetActive(true);

                break;
            case 1:
                _leftEngineFailure.SetActive(true);
                break;

            default:
                break;
        }

        if (playerLives == 0)
        {
            //Display the explosion animation
            Instantiate(_playerExplosionPrefab, transform.position, Quaternion.identity);
            _gameManager.UpdateGameStatus(false);
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Turns on the Tripleshot power up
    /// </summary>
    public void TripleShotOn()
    {
        canTripleShot = true;
        StartCoroutine(Tripleshotoff());
    }

    /// <summary>
    /// Turns off the triple shot power up after 5 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator Tripleshotoff()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

    /// <summary>
    /// Turns on the speed boost power up
    /// </summary>
    public void SpeedBoostOn()
    {
        _speed *= 2.0f;
        StartCoroutine(SpeedBoostOff());
    }

    /// <summary>
    /// Turns off the speed boost power up after 5 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpeedBoostOff()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= 2.0f;
    }

    /// <summary>
    /// Turns on the sheidlds
    /// </summary>
    public void ShieldOn()
    {
        _isShieldOn = true;
        _playerShield.SetActive(true);
    }

    #endregion
}

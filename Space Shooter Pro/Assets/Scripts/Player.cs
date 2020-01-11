using UnityEngine;

public class Player : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _lives = 3;

    [SerializeField] private GameObject _laserPrefab;

    private float _canFire = -1f;
    private SpawnManager _spawnManager;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null.");
        }

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
        Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y + 1.05f, 0);

        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, laserPosition, Quaternion.identity);
    }

    /// <summary>
    /// Damages the player
    /// </summary>
    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    #endregion
}

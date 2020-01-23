using System.Collections;
using UnityEngine;

public class RammingEnemy : Enemy
{
    #region Private variables

    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private LayerMask _powerUpLayerMask;
    [SerializeField] private AudioClip _missileSFX;
    [SerializeField] private GameObject _shieldVisualizer;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private int _minShiftWait = 2;
    private int _maxShiftWait = 4;
    private int _minShiftTime = 1;
    private int _maxShiftTime = 2;
    private int _shiftAmount = 1;
    private float _targetShift;
    private bool _canShootMissile = true;
    private bool _activateShields = false;
    private float _distanceToPlayer;
    private bool _doneRaming = true;
    private Vector3 _playerPosition;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        catch
        { }

        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _targetShift = 0;
        ActivateSheilds();
        StartCoroutine(EnemyShift());
        _distanceToPlayer = Mathf.Infinity;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();

        if (_distanceToPlayer <= 4)
            RamPlayer();

        if (_shootLaser == true)
        {
            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                ShootLaser();

                if (PowerUpAhead() == true)
                    ShootLaser();
            }

            if ((PlayerBehind() == true) && (_canShootMissile == true))
                ShootMissile();
        }
    }

    /// <summary>
    /// Collection detection for the enemy
    /// </summary>
    /// <param name="other">The colliding object</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Laser"))
        {
            Projectile projectile = other.GetComponent<Projectile>();

            if (projectile.IsEnemyProjectile() == true)
            {
                Destroy(other.gameObject);
            }
            else
            {
                if (_activateShields == false)
                {
                    _shootLaser = false;
                    Destroy(other.gameObject);
                    _speed = 0;
                    _anim.SetTrigger("OnEnemyDeath");
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    _audioSource.PlayOneShot(_explosionSFX);

                    if (_player != null)
                        _player.AddScore(_scoreValue);

                    Destroy(other.gameObject);
                    Destroy(this.gameObject, 2.6f);
                }
                else
                {
                    Destroy(other.gameObject);
                    _activateShields = false;
                    _shieldVisualizer.SetActive(false);
                }
            }
        }
        else if (other.gameObject.tag.Equals("Player"))
        {
            if (_activateShields == false)
            {
                _shootLaser = false;
                _speed = 0;
                _anim.SetTrigger("OnEnemyDeath");
                this.GetComponent<BoxCollider2D>().enabled = false;
                _audioSource.PlayOneShot(_explosionSFX);

                if (_player != null)
                {
                    _player.AddScore(_scoreValue);
                    _player.Damage();
                }

                Destroy(this.gameObject, 2.6f);
            }
            else
            {
                if (_player != null)
                    _player.Damage();

                _activateShields = false;
                _shieldVisualizer.SetActive(false);
            }
        }
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Moves the Enemy Downwards
    /// </summary>
    private void MoveEnemy()
    {
        Vector3 direction = new Vector3(_targetShift, -1, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
            _canShootMissile = true;
        }

        if (_doneRaming == true)
        {
            if (_player != null)
                _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        }
    }

    /// <summary>
    /// Rams the player if it is in close distance
    /// </summary>
    private void RamPlayer()
    {
        _doneRaming = false;

        if (_doneRaming == true)
            _playerPosition = _player.transform.position;

        Vector3 direction = new Vector3(_playerPosition.x - transform.position.x, -1, 0);
        direction.Normalize();

        transform.Translate(direction * Time.deltaTime);

        if (transform.position == _playerPosition)
            _doneRaming = true;
    }

    /// <summary>
    /// Shoots the enemy laser
    /// </summary
    private void ShootLaser()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_laserSFX);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
            lasers[i].AssignEnemyProjectile();
    }

    /// <summary>
    /// Shoots the enemy Missile
    /// </summary>
    private void ShootMissile()
    {
        GameObject enemyMissile = Instantiate(_missilePrefab, transform.position + new Vector3(0, 1.67f, 0), Quaternion.identity);
        _audioSource.PlayOneShot(_missileSFX);
        Projectile missile = enemyMissile.GetComponent<Projectile>();
        missile.AssignEnemyProjectile();
        _canShootMissile = false;
    }

    /// <summary>
    /// Checks whether there is a powerup in front of the enemy or not
    /// </summary>
    /// <returns>True if there is a power up and false otherwise.</returns>
    private bool PowerUpAhead()
    {
        RaycastHit2D hitInfo;
        hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 100.0f, _powerUpLayerMask);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.tag.Equals("PowerUp"))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the player is behind the enemy or not
    /// </summary>
    /// <returns>True if there is a player and false otherwise.</returns>
    private bool PlayerBehind()
    {
        RaycastHit2D hitInfo;
        hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 100.0f, _playerLayerMask);
        Debug.DrawRay(transform.position, Vector3.up, Color.red);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.tag.Equals("Player"))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Randomly decides if the enemy should activate their sheilds or not upon spawning
    /// </summary>
    private void ActivateSheilds()
    {
        int rand = Random.Range(0, 2);

        if (rand == 1)
        {
            _activateShields = true;
            _shieldVisualizer.SetActive(true);
        }
    }

    /// <summary>
    /// Makes the enemy shifts direction
    /// </summary>
    IEnumerator EnemyShift()
    {
        yield return new WaitForSeconds(Random.Range(_minShiftWait, _maxShiftWait));

        while (true)
        {
            _targetShift = _shiftAmount * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(_minShiftTime, _maxShiftTime));
            _targetShift = 0;
            yield return new WaitForSeconds(Random.Range(_minShiftTime, _maxShiftTime));
        }
    }

    #endregion
}

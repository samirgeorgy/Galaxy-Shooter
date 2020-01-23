using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    #region Private Variables

    [SerializeField] private int _bossHealth = 100;
    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private GameObject[] _bossLaserAttackPrefabs;
    [SerializeField] private Animator _bossExplosionAnimation;

    private AudioSource _enemyExplosionSFX;

    private float _canFire = -1f;
    private bool _bossEntryFinished = false;
    private Vector3 _playerCapturedLocation;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {       
        _enemyExplosionSFX = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (_player != null)
            _playerCapturedLocation = _player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bossEntryFinished == false)
            PlayBossEntry();
        else
            Maneuver();

        if (_player.IsPlayerDead() == false)
            Attack();
    }

    /// <summary>
    /// Plays the entry of the boss
    /// </summary>
    private void PlayBossEntry()
    {
        if (transform.position.y > 4.7f)
            transform.Translate(Vector3.down * 2 * Time.deltaTime);
        else
        {
            _bossEntryFinished = true;
            UIManager.Instance.EnableBossLivesText();
        }  
    }
    
    /// <summary>
    /// Collision Detection
    /// </summary>
    /// <param name="other">The collided object</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Laser"))
        {
            Projectile projectile = other.GetComponent<Projectile>();

            if (projectile == null)
                return;

            if (projectile.IsEnemyProjectile() == false)
            {
                Destroy(other.gameObject);
                TakeDamage();
            }
        }
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Defines the boss movement system
    /// </summary>
    private void Maneuver()
    {
        if (_player != null)
        {
            _playerCapturedLocation = _player.transform.position;

            if (transform.position.x > _playerCapturedLocation.x)
                StartCoroutine(MoveTowardsPlayer(1));
            else if (transform.position.x < _playerCapturedLocation.x)
                StartCoroutine(MoveTowardsPlayer(0));
        }
    }

    /// <summary>
    /// Give Damage to the boss
    /// </summary>
    private void TakeDamage()
    {
        _bossHealth -= 10;
        UIManager.Instance.UpdateBossLives(_bossHealth);

        if (_bossHealth < 1)
        {
            UIManager.Instance.DisableBossLivesText();
            UIManager.Instance.UpdateBossLives(100);
            _shootLaser = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            _speed = 0;
            _enemyExplosionSFX.Play();
            _bossExplosionAnimation.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.6f);
        }
    }

    /// <summary>
    /// The boss attacks
    /// </summary>
    private void Attack()
    {
        if (_shootLaser == true)
        {
            GameObject enemyLaser = null;

            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(1f, 2f);
                _canFire = Time.time + _fireRate;
                
                if (_bossHealth > 40)
                    enemyLaser = Instantiate(_bossLaserAttackPrefabs[0], transform.position, Quaternion.identity);
                else
                    enemyLaser = Instantiate(_bossLaserAttackPrefabs[1], transform.position, Quaternion.identity);

                if (enemyLaser != null)
                {
                    Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                    for (int i = 0; i < lasers.Length; i++)
                        lasers[i].AssignEnemyProjectile();
                }
            }
        }
    }

    /// <summary>
    /// Makes the boss wait for some seconds and then moves towards the x value of the player
    /// </summary>
    /// <param name="direction">0 indicates movement to the right and 1 to the left</param>
    IEnumerator MoveTowardsPlayer(int direction)
    {
        yield return new WaitForSeconds(0.5f);

        if (direction == 0)
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        else if (direction == 1)
            transform.Translate(Vector3.left * _speed * Time.deltaTime);

    }

    #endregion
}

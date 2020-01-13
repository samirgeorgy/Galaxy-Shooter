using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Private variables

    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private int _scoreValue = 10;
    [SerializeField] private GameObject _laserPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private AudioSource _audioSource;
    private Player _player;
    private Animator _anim;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
        
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignEnemyLaser();
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
            Destroy(other.gameObject);
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            this.GetComponent<BoxCollider2D>().enabled = false;
            _audioSource.Play();

            if (_player != null)
                _player.AddScore(_scoreValue);

            Destroy(this.gameObject, 2.6f);
        }
        else if (other.gameObject.tag.Equals("Player"))
        {
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            this.GetComponent<BoxCollider2D>().enabled = false;
            _audioSource.Play();

            if (_player != null)
            {
                _player.AddScore(_scoreValue);
                _player.Damage();
            }

            Destroy(this.gameObject, 2.6f);
        }
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Moves the Enemy Downwards
    /// </summary>
    private void MoveEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    #endregion
}
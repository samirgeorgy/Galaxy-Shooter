using System.Collections;
using UnityEngine;

public class ManuveringEnemy : Enemy
{
    #region Private variables

    [SerializeField] private int _frequency = 3;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        catch { }
        
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(ShootLaserBeam());
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
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
        }
        else if (other.gameObject.tag.Equals("Player"))
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
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Moves the Enemy Downwards
    /// </summary>
    private void MoveEnemy()
    {
        Vector3 direction = new Vector3(Mathf.Sin(Time.time * _frequency), -1, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    /// <summary>
    /// Shoots the enemy laser
    /// </summary>
    private void ShootLaser()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_laserSFX);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
            lasers[i].AssignEnemyProjectile();
    }

    IEnumerator ShootLaserBeam()
    {
        yield return new WaitForSeconds(Random.Range(1, 2));

        while (true)
        {
            if (_shootLaser == true)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (_shootLaser == true)
                        ShootLaser();
                    else
                        break;

                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(Random.Range(1, 2));
            }
            else
                break;
        }
    }

    #endregion
}

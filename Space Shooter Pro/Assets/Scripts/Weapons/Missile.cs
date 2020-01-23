using UnityEngine;

public class Missile : Projectile
{
    #region Private Variables

    [SerializeField] float _rotateSpeed = 300;

    private Enemy _targetEnemy;
    private Transform _player;
    private Rigidbody2D _rigidbody;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_player != null)
            TargetClosestEnemy();
    }


    // Update is called once per frame
    private void Update()
    {
        if (_isEnemyProjectile == true)
            MoveMissile();
    }

    void FixedUpdate()
    {
        if (_isEnemyProjectile == false)
            MoveMissileTowardsEnemy();
    }

    /// <summary>
    /// Collision Detection
    /// </summary>
    /// <param name="collision">The collider object</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && (_isEnemyProjectile == true))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Targets the closest enemy
    /// </summary>
    private void TargetClosestEnemy()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        Enemy eMin = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = _player.position;

        foreach(GameObject enemy in enemyList)
        {
            Enemy en = enemy.GetComponent<Enemy>();

            if (en != null)


            if (en != null)
            {
                if (en.IsTargetted() == false)
                {
                    float dist = Vector3.Distance(enemy.transform.position, currentPos);

                    if (dist < minDistance)
                    {
                        eMin = en;
                        minDistance = dist;
                    }
                }
            }
        }

        _targetEnemy = eMin;

        if (_targetEnemy != null)
            _targetEnemy.SetTarget();
    }

    /// <summary>
    /// Moves the Missile
    /// </summary>
    private void MoveMissileTowardsEnemy()
    {
        if (_targetEnemy != null)
        {
            Vector2 direction = (Vector2)_targetEnemy.transform.position - _rigidbody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _rigidbody.angularVelocity = -rotateAmount * _rotateSpeed;
            _rigidbody.velocity = transform.up * _speed;
        }
        else
            MoveMissile();
    }

    /// <summary>
    /// Moves the missile upwards
    /// </summary>
    private void MoveMissile()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
    }

    #endregion
}

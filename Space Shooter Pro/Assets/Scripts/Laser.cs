using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _speed = 8.0f;
    private bool _isEnemyLaser = false;

    #endregion

    #region Unity Functions

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
            MoveLaserUp();
        else
            MoveLaserDown();
    }

    /// <summary>
    /// Collision Detection
    /// </summary>
    /// <param name="collision">The collider object</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && (_isEnemyLaser == true))
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
    /// Moves the laser upwards
    /// </summary>
    private void MoveLaserUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Moves the laser downwards
    /// </summary>
    private void MoveLaserDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Marks the laser as enemy laser
    /// </summary>
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    /// <summary>
    /// Checks whether the laser is of the enemy or not.
    /// </summary>
    /// <returns>True if the laser is from an enemy and false if not.</returns>
    public bool IsEnemyLaser()
    {
        return _isEnemyLaser;
    }

    #endregion
}

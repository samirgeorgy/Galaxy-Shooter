using UnityEngine;

public class Laser : Projectile
{
    #region Unity Functions

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyProjectile == false)
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

    #endregion
}

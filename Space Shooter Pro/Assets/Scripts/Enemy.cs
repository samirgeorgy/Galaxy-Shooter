using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Private variables

    [SerializeField] private float _speed = 4.0f;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        
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
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag.Equals("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
                player.Damage();

            Destroy(this.gameObject);
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

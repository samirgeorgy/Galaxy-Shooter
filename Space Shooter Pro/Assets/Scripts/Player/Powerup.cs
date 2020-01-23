using UnityEngine;

public class Powerup : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerupID;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioClip _explosionSFX;
    [SerializeField] private GameObject _explosionEffect;
    
    private Transform _player;
    private bool _moveTowardsPlayer = false;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            _moveTowardsPlayer = true;

        if (_moveTowardsPlayer == false)
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        else
        {
            Vector3 direction = _player.position - transform.position;
            direction.Normalize();

            transform.Translate(direction * _speed * 2 * Time.deltaTime);
        }

        if (transform.position.y < -4.5)
            Destroy(this.gameObject);
    }

    //Collision detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Player player = collision.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.ReloadAmmo();
                        break;
                    case 4:
                        player.AddHealth();
                        break;
                    case 5:
                        player.MayhemActive();
                        break;
                    case 6:
                        player.EngineCrumble();
                        break;
                    case 7:
                        player.AddMissileAmmo();
                        break;
                    default:
                        break;
                }
            }

            Destroy(this.gameObject);
        }
        else if (collision.tag.Equals("Laser"))
        {
            Projectile projectile = collision.GetComponent<Projectile>();

            if (projectile.IsEnemyProjectile())
            {
                AudioSource.PlayClipAtPoint(_explosionSFX, transform.position);
                Instantiate(_explosionEffect, transform.position, Quaternion.identity);
                this.enabled = false;
                this.gameObject.SetActive(false);
                Destroy(collision.gameObject);
                Destroy(this.gameObject, 3.0f);
            }
        }
    }

    #endregion
}

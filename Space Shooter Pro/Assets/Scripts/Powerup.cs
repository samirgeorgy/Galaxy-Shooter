using UnityEngine;

public class Powerup : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerupID;
    [SerializeField] private AudioClip _clip;

    #endregion

    #region Unity Functions

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

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
                    default:
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }

    #endregion
}

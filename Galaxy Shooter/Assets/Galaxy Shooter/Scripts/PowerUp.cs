using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    #region Private Variables

    //The speed of the power up
    [SerializeField]
    private float _speed = 3.0f;

    //The ID of the power up
    [SerializeField]
    private int _powerUpID; //0 = Triple Shot; 1 = Speed boost; 2 = Sheilds

    //The power up audio clip
    [SerializeField]
    private AudioClip _audioClip;

    #endregion

    #region API Methods
	
	// Update is called once per frame
	void Update () {

        //Moving the power up
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.9f)
            Destroy(this.gameObject);
	}

    /// <summary>
    /// Applies the triple shot power up once it collides with the player
    /// </summary>
    /// <param name="other">the object that we collided with</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Get The player
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                //Enable the Triple shot power up.
                if (_powerUpID == 0)
                    player.TripleShotOn();

                //Enable the Speed boost power up
                if (_powerUpID == 1)
                    player.SpeedBoostOn();

                //Enable the sheilds
                if (_powerUpID == 2)
                    player.ShieldOn();

                AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1.0f);

                //Destroy the power up icon
                Destroy(this.gameObject);
            }
        }
    }

    #endregion
}

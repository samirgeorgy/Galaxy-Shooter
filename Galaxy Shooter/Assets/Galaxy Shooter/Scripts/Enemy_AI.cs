using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour {

    #region Private Variables

    //Enemy Speed
    [SerializeField]
    private float _speed = 5.0f;

    //The enemy explosion
    [SerializeField]
    private GameObject _enemyExplosionPrefab;

    //The UI Manager
    UIManager _uiManager;

    //The audio source of the explosion
    [SerializeField]
    private AudioClip _audioClip;

    #endregion

    // Use this for initialization
    void Start () {

        //When an enemy is spawned, the initial location should be random
        transform.position = new Vector3(Random.Range(-7.9f, 7.9f), 6.2f, 0);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
	
	// Update is called once per frame
	void Update () {

        //Move the enemy
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.2f)
            transform.position = new Vector3(Random.Range(-7.9f, 7.9f), 6.2f, 0);
	}

    /// <summary>
    /// This function checks the collition of the enemy with the player
    /// </summary>
    /// <param name="other">the game object of the player</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (other.transform.parent != null)
                Destroy(other.transform.parent.gameObject);

            //Before destroying the enemy, we will display the explosion
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);

            if (_uiManager != null)
                _uiManager.UpdateScore();

            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            //If the collision happend with the player, we decrease its lives by 1
            if (player != null)
                player.DamagePlayer();

            //Before destroying the enemy, we will display the explosion
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
    }
}

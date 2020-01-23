using UnityEngine;

public class Astroid : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _rotationSpeed = 19f;
    [SerializeField] private GameObject _explostionPrefab;

    #endregion

    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Check for collision
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Laser"))
        {
            GameObject explosion = Instantiate(_explostionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            SpawnManager.Instance.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
    }

    #endregion
}

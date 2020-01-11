using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private float _speed = 8.0f;

    #endregion

    #region Unity Functions

    // Update is called once per frame
    void Update()
    {
        MoveLaser();

        if (transform.position.y > 8f)
            Destroy(this.gameObject);
    }

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Moves the laser upwards
    /// </summary>
    private void MoveLaser()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    #endregion
}

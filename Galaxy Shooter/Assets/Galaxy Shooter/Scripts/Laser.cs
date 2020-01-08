using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    #region Private Variables

    [SerializeField]
    private float _speed = 10.0f;

    #endregion

    #region API Methods

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Moving the laser forward on the Y-axis
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        if (transform.position.y > 6)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
	}

    #endregion
}

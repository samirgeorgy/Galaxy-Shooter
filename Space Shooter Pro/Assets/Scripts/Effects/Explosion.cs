﻿using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3f);
    }

    #endregion
}

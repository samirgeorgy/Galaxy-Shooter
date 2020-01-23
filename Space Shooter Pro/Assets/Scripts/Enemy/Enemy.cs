using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Private variables

    [SerializeField] protected float _speed = 4.0f;
    [SerializeField] protected int _scoreValue = 10;
    [SerializeField] protected GameObject _laserPrefab;

    [SerializeField] protected AudioClip _laserSFX;
    [SerializeField] protected AudioClip _explosionSFX;

    protected bool _shootLaser = true;
    protected bool _isTargeted = false;

    protected AudioSource _audioSource;
    protected Player _player;
    protected Animator _anim;

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Sets the enemy as a target for a missile
    /// </summary>
    public void SetTarget()
    {
        _isTargeted = true;
    }

    /// <summary>
    /// Checks whether the enemy is targetted by a missile or not
    /// </summary>
    public bool IsTargetted()
    {
        return _isTargeted;
    }

    #endregion
}
using UnityEngine;

/// <summary>
/// Parent Class of any Projectile Weapon
/// </summary>
public class Projectile : MonoBehaviour
{
    #region Private Variables

    [SerializeField] protected float _speed;
    [SerializeField] protected bool _isEnemyProjectile = false;

    #endregion

    #region Supporting Functions

    /// <summary>
    /// Assigns the projectile to the enemy
    /// </summary>
    public void AssignEnemyProjectile()
    {
        _isEnemyProjectile = true;
    }

    /// <summary>
    /// Checks whether the projectile is from an enemy or not
    /// </summary>
    /// <returns>True if the projectile is from an enemy, False otherwise</returns>
    public bool IsEnemyProjectile()
    {
        return _isEnemyProjectile;
    }

    #endregion
}

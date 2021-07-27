using System;
using UnityEngine;

/// <summary>
/// Handles Invader collisions and missile drops
/// </summary>
public class Invader : MonoBehaviour
{
    #region PUBLIC_VARS
    /// <summary>
    /// Missile prefab for random drop Instantiation
    /// </summary>
    public Missile missilePrefab;

    /// <summary>
    /// Score value of this invader when player hits
    /// </summary>
    public int Score;

    /// <summary>
    /// Callback when invader is shot down
    /// </summary>
    public Action<Invader> Killed;
    #endregion

    #region PRIVATE_VARS
    private bool missileActive;
    private CollisionHandler CollisionChild;
    #endregion

    #region METHODS
    private void Awake()
	{
        //Get collision handler child to handle the laser triggers
        CollisionChild = GetComponentInChildren<CollisionHandler>();
        CollisionChild.TriggerEntered = TriggerEntered;
    }

    public void Drop()
    {
        //Drop a new missile if old one is died
        if (!this.missileActive)
        {
            this.missileActive = true;

            Missile missile = Instantiate(missilePrefab, this.transform.position, Quaternion.identity);           
            missile.destroyed += OnLaserDestroyed;
        }
    }

    private void OnLaserDestroyed(Missile missile)
    {
        //set false to drop a new one
        missileActive = false;
    }


    private void TriggerEntered(Collider other)
    {
        //Ignore the missiles collision dropped by other invaders
        if (other.gameObject.tag == "Missile")
        {
            Physics.IgnoreCollision(other, CollisionChild.GetComponent<Collider>());
            return;
        }

        //If its Laser then kill this invader 
        Killed?.Invoke(this);

        //Also destroy the laser object by invoking Kill
        other.transform.parent.GetComponent<Laser>().Kill();	
    }
    #endregion
}

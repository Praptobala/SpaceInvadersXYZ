using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Laser shot movements
/// </summary>
public class Laser : MonoBehaviour
{
    #region PUBLIC_VARS
    /// <summary>
    /// Speed of the laser movement
    /// </summary>
    public float speed = 30.0f;

    /// <summary>
    /// Direction of laser shot
    /// </summary>
    public Vector3 direction = Vector3.up;

    /// <summary>
    /// Callback when laser object is destroyed
    /// </summary>
    public System.Action<Laser> destroyed;
    #endregion

    #region PUBLIC_VARS
    private void OnDestroy()
    {
        destroyed?.Invoke(this);
    }

	private void Update()
    {
        // Move based on given speed/direction
        transform.position += this.direction * this.speed  * Time.deltaTime;

        //Destroy the object when it reached beyond the boundry
        if (transform.position.y > Settings.YBound)
            Kill();
    }    

    public void Kill()
	{
        Destroy(gameObject);
    }
    #endregion
}

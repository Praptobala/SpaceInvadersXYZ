using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Missile movements and collisions
/// </summary>
public class Missile : MonoBehaviour
{
    #region PUBLIC_VARS
    /// <summary>
    /// Enum declarion types of Missile styles
    /// </summary>
    public enum Style { Slow = 0, Fast, Wiggly }

    /// <summary>
    /// This Missile style 
    /// </summary>
    public Style Type = Style.Slow;  

    /// <summary>
    /// Direction of the missile movement
    /// </summary>
    public Vector3 direction = Vector3.down;

    /// <summary>
    /// Callback when missile is destroyed
    /// </summary>
    public System.Action<Missile> destroyed;
    #endregion

    #region PRIVATE_VARS
    //Downward movement speed
    private float speed = 20.0f;

    //Wiggle Movement
    private float frequency = 20.0f;  // Speed of sine movement
    private float magnitude = 0.03f;   // Size of sine movement
    #endregion

    #region METHODS
    private void Start()
	{
        //Tag the trigger name as Missile
        gameObject.tag = "Missile";

        // Decides the speed of drop based on style
		switch (Type)
		{
            case Style.Fast:
                speed = 10.0f;
                break;
            case Style.Wiggly:
                speed = 5.0f;
                break;
			default:
                speed = 5.0f;
                break;
        }       
    }

	private void OnDestroy()
    {       
        destroyed?.Invoke(this);
    }

	private void Update()
    {
        //Move the missile with given direction and speed
        transform.position += direction * speed  * Time.deltaTime;

        //Add wiggle in movement if the missile type is Wiggly
        if (Type == Style.Wiggly)
            transform.position += transform.right * Mathf.Sin(Time.time * frequency) * magnitude;

        //Kill the missile if it goes beyond screen
        if (transform.position.y < -1.0f)
            Kill();
    }    

    public void Kill()
	{  
        Destroy(gameObject);
    }
    #endregion
}

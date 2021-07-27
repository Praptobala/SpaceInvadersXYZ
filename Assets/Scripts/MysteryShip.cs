using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles MysteryShip movements and collisions
/// </summary>
public class MysteryShip : MonoBehaviour
{
    #region PUBLIC_VARS
    /// <summary>
    /// Types of MysteryShip ENUM 
    /// </summary>
    public enum PowerUpType { Life = 0, Thunder, Shield  }

    /// <summary>
    /// This MysteryShip PowerUp Type 
    /// </summary>
    public PowerUpType Type;

    /// <summary>
    /// Callback Action when Ship is shot down
    /// </summary>
    public Action<PowerUpType> PowerUp;

    /// <summary>
    /// Moving Speed
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// Appear on screen intervel in Secs
    /// </summary>
    public float appearTimeSpan = 20.0f;
    #endregion


    #region PRIVATE_VARS
    private Vector3 StartPos;
    private Vector3 EndPos;
    private bool showUp;
    #endregion

    #region METHODS
    private void Start()
    {
        //Left position of the ship
        StartPos =  new Vector3(-Settings.XBound - 1.0f, transform.position.y, transform.position.z);

        //Right position of the ship
        EndPos = new Vector3(Settings.XBound + 1.0f, transform.position.y, transform.position.z);

        //ShowUp the ship on screen
        Run();
    }

    private void Run()
    {
        //Reset pos to left end
        transform.position = StartPos;

        //Release the update by setting flag true        
        showUp = true;
    }

    private void Stop()
    {
        //Reset pos to left end
        transform.position = StartPos;

        //Lock the update to stop moving the ship
        showUp = false;

        //Call the Run method to restart the movement after sometime
        Invoke(nameof(Run), appearTimeSpan);
    }

    private void Update()
    { 
        if (!showUp)
            return;
        
        //Move the ship from left to right on given speed
        transform.position += Vector3.right * speed * Time.deltaTime;
       
        // stop the ship movement & hide when reached right end
        if (transform.position.x >= EndPos.x)
            Stop();
    }

    private void OnTriggerEnter(Collider other)
	{
        // stop the ship movement & hide when hit by player shot
        Stop();

        //Invke callback to power-up the player
        PowerUp?.Invoke(Type);               
	}
    #endregion
}

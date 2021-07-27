using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Player movements, collisions & Laser shoot
/// </summary>
public class PlayerBase : MonoBehaviour
{
    #region PUBLIC_VARS
    /// <summary>
    /// Player movement speed
    /// </summary>
    public float speed = 5.0f;

    /// <summary>
    /// Laser prefab for cloning every shoot
    /// </summary>
    public Laser laserPrefab;

    /// <summary>
    /// Callback when player is shot down
    /// </summary>
    public System.Action killed;
    #endregion

    #region PRIVATE_VARS
    private bool laserActive { get; set; }
    private CollisionHandler CollisionChild;
    private Vector3 backupPos;
    private bool inactive = false;
    #endregion

    #region METHODS
    private void Awake()
    {
        //Get collision handler child to handle the missile triggers
        CollisionChild = GetComponentInChildren<CollisionHandler>();      
        CollisionChild.TriggerEntered = TriggerEntered;
    }

    private void Update()
    {
        //Do not handle update for few secs when killed
        if (inactive)
            return;

        Vector3 position = transform.position;

        // Move player base left/Right based on user input 
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= this.speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            position.x += speed * Time.deltaTime;
        }

        // Restrict the Left/Right bount with in the bound limit
        position.x = Mathf.Clamp(position.x, -(Settings.XBound - 1), Settings.XBound - 1);
        transform.position = position;

        //Shoot a new laser when spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    private void Shoot()
    {        
        //Shoot a laser at a time/ wait for old one to die
        if (!laserActive)
        {
            laserActive = true;

            Laser laser = Instantiate(laserPrefab, this.transform.position, Quaternion.identity);
            laser.destroyed += OnLaserDestroyed;
        }
    }

    //Hide the Player Base for 2 secs after killed
    private void Hide()
	{
        inactive = true;
        //Backup the old pos
        backupPos = transform.position;

        //Hide the player out of bound
        var temp = transform.position;
        temp.x = -Settings.XBound - 1;
        transform.position = temp;

        //call Show after 1 sec
        Invoke("Show", 1.0f);
	}

    //Backup showing the PlayerBase in the same position
    private void Show()
	{
        // Show the player back in same pos
        transform.position = backupPos;
        inactive = false;
    }

    private void OnLaserDestroyed(Laser laser)
    {       
        laserActive = false;
    }

    private void TriggerEntered(Collider collider)
    {        
        //Destroy the missile by calling Kill
        collider.transform.parent.GetComponent<Missile>().Kill();        

        //Invoke Killed callback for GameManager update
        killed?.Invoke();

        //Hide the PlayerBase for few secs
        Hide();
    }
    #endregion
}

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles forming Invaders and Stepping
/// </summary>
public class InvadersGroup : MonoBehaviour
{
    #region PUBLIC_VARS
    /// <summary>
    /// Array holds different types of Invader prefabs
    /// </summary>
    public Invader[] prefabs = new Invader[5];

    /// <summary>
    /// Number of Invader Rows
    /// </summary>
    public int rows = 5;

    /// <summary>
    /// Number of Invader Columns
    /// </summary>
    public int columns = 11;

    /// <summary>
    /// Speed of the entire invaders grid move 
    /// </summary>
    public float speed = 1f;

    /// <summary>
    /// Callback to send Invader score
    /// </summary>
    public Action<int> InvaderScore;

    /// <summary>
    /// Callback when all invaders are killed
    /// </summary>
    public Action AllInvadersKilled;

    /// <summary>
    /// Callback when grid hits left/right edge
    /// </summary>
    public Action<int> StepDown;
    #endregion

    #region PRIVATE_VARS
    private Vector3 direction = Vector3.right;
    private int KilledCount;
    private int step = 1;
    private float waitTime = 2.0f;
    private float timer = 0.0f;
    private List<Invader> InvadersList = new List<Invader>();
    #endregion

    #region METHODS
    public void RollOut(int stepCount)
	{
        //Initialize the entire invader grid 
        step = stepCount;

        //Horizantal gap between each invader 
        float gapX = Settings.InvaderGridX;
        //Vertical gap between each invader 
        float gapY = Settings.InvaderGridY;

        // Form a grid of invaders based on number of rows & cols
        for (int i = 0; i < rows; i++)
        {         
            // Calculate width/height based on gap values given
            float width = gapX * (columns - 1);
            float height = gapY * (rows - 1);

            // Finding the center position of the grid
            Vector2 center = new Vector2(-width * 0.5f, -height * 0.5f);

            // Finding row pos from center & step params  
            Vector3 rowPosition = new Vector3(center.x, (gapY * i) + center.y - (step * Settings.StepVerticalSpeed), 0.0f);

            for (int j = 0; j < columns; j++)
            {  
                //instantiate the invader prefab 
                Invader invader = Instantiate(prefabs[i], transform);                
                invader.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                invader.Killed += OnInvaderKilled;

                //Store all the invaders for random missile drop 
                InvadersList.Add(invader);

                //Update the X pos of the invader
                Vector3 position = rowPosition;
                position.x += gapX * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void OnInvaderKilled(Invader invader)
    {
        //Callback received whe invader is killed

        //Set the Invader object inactive
        invader.gameObject.SetActive(false);

        //Increase the kill count
        KilledCount++;

        //Invoke UI CB to update the score
        InvaderScore?.Invoke(invader.Score);

        //Invoke UI CB when all invaders are killed
        if (KilledCount >= InvadersList.Count)
            AllInvadersKilled?.Invoke();
    }

    private void Update()
    {
        //Calculate the speed based on the number of killed invaders/ speed increase when invaders are killed
        float speed_invaders = KilledCount > 0?(speed + (Settings.StepHorizantalSpeed * KilledCount)) : speed;

        //Move the entire frid based on the direction and speed
        transform.position += direction * speed_invaders * Time.deltaTime;

        //Check if any one of the invader touches right/left edge of the screen
        foreach (Transform invader in transform)
        {
            //Ignore the invader not active/killed
            if (!invader.gameObject.activeInHierarchy)
                continue;

            //Move the grid down one step when hits the right/left edge
            if ((direction == Vector3.right && invader.position.x >= (Settings.XBound - 1.0f)) ||
                (direction == Vector3.left && invader.position.x <= (-Settings.XBound + 1.0f)))
            {                
                MoveOneStepDown();
                break;
            } 
        }

        // Timer to drop a missile on every 2 sec
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            timer = timer - waitTime;
            RandomMissileDrop();            
        }
       
    }

    private void MoveOneStepDown()
    {
        //Increment the horizantal step move the grid down
        step += 1;

        // Flip the direction
        direction = new Vector3(-direction.x, 0.0f, 0.0f);

		// Step down the entire invader grid
		Vector3 position = transform.position;
        position.y -= Settings.StepVerticalSpeed;		
        transform.position = position;

        //Send callback for shelter collision check 
        StepDown?.Invoke(step);
    }

    private void RandomMissileDrop()
	{
        //While loop breaks when one missile is dropped
        while(true)
		{
            //Get random index to choose which invader to drop the missile
            int invader_idx = UnityEngine.Random.Range(0, InvadersList.Count - 1);

            //Check if invader is active in the scene
            if(InvadersList[invader_idx].isActiveAndEnabled)
			{
                InvadersList[invader_idx].Drop();
                break;
            }
        }
	}
    #endregion
}

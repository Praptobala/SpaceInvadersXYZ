using UnityEngine;

/// <summary>
/// Main settings class just contains static vals
/// Can also be converted to a serializable class
/// to store the setting params as Json
/// </summary>
public class Settings : MonoBehaviour
{
	/// <summary>
	/// Horizantal edge of Screen 
	/// </summary>
	public static float XBound = 10.0f;

	/// <summary>
	/// Vertical top edge of Screen 
	/// </summary>
	public static float YBound = 12.0f;

	/// <summary>
	/// Horizantal gap b/w the invaders  
	/// </summary>
	public static float InvaderGridX = 0.8f;

	/// <summary>
	/// Vertical gap b/w the invaders  
	/// </summary>
	public static float InvaderGridY = 0.8f;

	/// <summary>
	/// Number total steps down to invade the player base  
	/// </summary>
	public static int TotalSteps = 11;

	/// <summary>
	/// Vertical value grid move when each step down  
	/// </summary>
	public static float StepVerticalSpeed = 0.8f;

	/// <summary>
	/// Horizantal speed of entire invader grid  
	/// </summary>
	public static float StepHorizantalSpeed = 0.02f;

	/// <summary>
	/// Max lives count when at start of the game  
	/// </summary>
	public static int LivesCount = 3;

	/// <summary>
	/// Max number of levels in the game  
	/// </summary>
	public static int LevelsCount = 9;

	/// <summary>
	/// Life count increment when player reaches the score   
	/// </summary>
	public static int LifeScore = 10000;

	/// <summary>
	/// Top High Score  
	/// </summary>
	public static int HighScore = 0;

	/// <summary>
	/// Total number of high score to be maintained in table  
	/// </summary>
	public static int HighScoreCount = 5;

	/// <summary>
	/// HighScore table playerpref string  
	/// </summary>
	public static string HighScoreTableString = "HIGH_SCORE_TABLE";
}

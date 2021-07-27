using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles all the Game Events and controls
/// entire game scene
/// </summary>
public class GameManager : MonoBehaviour
{
	#region PUBLIC_VARS
	/// <summary>
	/// The Prefab contains all main game components 
	/// </summary>
	[SerializeField]
	GameObject LevelPrefab;

	/// <summary>
	/// UI Text to display High score
	/// </summary>
	[SerializeField]
	Text HighScoreText;

	/// <summary>
	/// UI Text to display player score
	/// </summary>
	[SerializeField]
	Text ScoreText;

	/// <summary>
	/// UI Text to display High score
	/// </summary>
	[SerializeField]
	Text LivesCountText;

	/// <summary>
	/// UI Text to display Screen Level
	/// </summary>
	[SerializeField]
	Text ScreenLevelText;

	/// <summary>
	/// UI Panel to display end of game
	/// </summary>
	[SerializeField]
	GameObject GameOverPanel;

	/// <summary>
	/// UI TextMeshPro to display High Score Table
	/// </summary>
	[SerializeField]
	TMP_Text HighScoreListText;


	/// <summary>
	/// HighScore table class to store the list of scores
	/// with PlayerPrefs
	/// </summary>
	[Serializable]
	public class HighScoreTable
	{
		public List<int> ScoreTable;
	}
	#endregion


	#region PRIVATE_VARS
	private GameObject Level;
	private GameObject shelters;
	private int PlayerLives = 0;
	private int GameLevel = 1;
	private int Score = 0;	
	private bool IsGameOver = false;
	private HighScoreTable highScoreTable = new HighScoreTable();
	#endregion


	#region METHODS
	private void Start()
	{
		// Load the stored High Score Data
		LoadPlayerPref();

		// Initialize the variables and text objects
		GameReset();

		// Instantiate game to Play
		StartGame();
	}

	private void LoadPlayerPref()
	{
		// Load the HighScore table list stored as Json string
		var table = PlayerPrefs.GetString(Settings.HighScoreTableString);
		highScoreTable = JsonUtility.FromJson<HighScoreTable>(table);

		//First Time No values availble so initialize here
		if (highScoreTable == null)
		{
			highScoreTable = new HighScoreTable();
			highScoreTable.ScoreTable = new List<int>();
		}

		//Get the highest value from the list to show as HighScore text
		if(highScoreTable.ScoreTable.Count > 0)
		{
			Settings.HighScore = highScoreTable.ScoreTable[highScoreTable.ScoreTable.Count - 1];
			HighScoreText.text = Settings.HighScore.ToString();
		}
	}

	private void SavePlayerPref()
	{
		// Save the Highscore table list as Json string
		var table = JsonUtility.ToJson(highScoreTable);
		PlayerPrefs.SetString(Settings.HighScoreTableString, table);
		PlayerPrefs.Save();

	}

	private void GameReset()
	{
		// Reset all values back to default
		PlayerLives = Settings.LivesCount;
		GameLevel = 1;
		Score = 0;

		// Reset all text values assigning default
		LivesCountText.text = PlayerLives.ToString();
		ScoreText.text = Score.ToString();
		ScreenLevelText.text = GameLevel.ToString();
		HighScoreListText.text = String.Empty;
	}

	private void StartGame()
	{		
		// Instantiate Main Level gameobject from Prefab		
		Level = Instantiate(LevelPrefab);

		// Get InvadersGroup comp & assign all CB funtions to consume game events
		var invaders = Level.GetComponentInChildren<InvadersGroup>();
		invaders.InvaderScore = PlayerScore;
		invaders.AllInvadersKilled = NextScreen;
		invaders.StepDown = StepDown;
		//Call this method to assign Screen/Level step count to form grid position
		invaders.RollOut(GameLevel);

		//Get Player/MystryShip/Shelter components to consume their events
		var player = Level.GetComponentInChildren<PlayerBase>();
		player.killed = PlayerKilled;

		var mysteryShip = Level.GetComponentInChildren<MysteryShip>();
		mysteryShip.PowerUp = PowerUpPlayer;

		shelters = Level.GetComponentInChildren<SheltersGroup>().gameObject;
	}
	
	private void PowerUpPlayer(MysteryShip.PowerUpType type)
	{
		//Called when Player shoots the Mystery Ship

		//Increase the Player life count if Powerup type is Life
		if (type == MysteryShip.PowerUpType.Life)
		{
			PlayerLives++;
			LivesCountText.text = PlayerLives.ToString();
		}
		else if (type == MysteryShip.PowerUpType.Thunder) // TO BE IMPLEMENTED - Other PowerUp Type
			Debug.Log("Thunder - Power UP");
		else if (type == MysteryShip.PowerUpType.Shield) // TO BE IMPLEMENTED - Other PowerUp Type
			Debug.Log("Thunder - Sheild UP");
	}

	private void PlayerScore(int score)
	{
		//Called when Player shoots an invader
		//Add up the score
		Score += score;

		//Chevk if the score reached more than high
		if (Settings.HighScore < Score)		
			Settings.HighScore = Score;		

		//Increase Player life when score reaches every 10K
		if (Score % Settings.LifeScore == 0)
			PlayerLives++;

		// Assign the values to respective UI Elements
		ScoreText.text = Score.ToString();
		HighScoreText.text = Settings.HighScore.ToString();
	}

	private void PlayerKilled()
	{
		//Called when Player is killed

		// Decrease the player lives count and update display
		PlayerLives--;
		LivesCountText.text = PlayerLives.ToString();

		// Call GameOver when no more lives
		if (PlayerLives == 0)
			GameOver();
	}

	private void NextScreen()
	{		
		//Called when Player show all the Invaders

		//Advance the level & update the display
		GameLevel++;
		ScreenLevelText.text = GameLevel.ToString();

		// Back to first level when level reaches last
		if (GameLevel > Settings.LevelsCount)
			GameLevel = 1;

		// Destroy existing Main level objects
		Destroy(Level);

		// Restart the game
		StartGame();
	}

	private void StepDown(int step)
	{
		//Called when Invaders grid hits left/right edges

		//Remove all the shelters if Invader grid reached down 7 step
		//Currently hard coded due to time constraint
		if (step > 7)
			shelters.SetActive(false);

		//Call Gameover when grid is 11 steps down 
		if (step > Settings.TotalSteps)
			GameOver();
	}

	private void GameOver()
	{
		// Destroy existing Main level objects
		Destroy(Level);

		// Set the gameover flag to enable press return to continue
		IsGameOver = true;

		// Show up the gameover panel
		GameOverPanel.gameObject.SetActive(true);

		//Process & display the high score table at GameOver panel
		DisplayHighScoreTable();

		//Finaly save the latest highscore table values
		SavePlayerPref();
	}

	private void AddToTable(int highScore)
	{
		//Add the latest high score into list
		highScoreTable.ScoreTable.Add(highScore);
		highScoreTable.ScoreTable.Sort();

		//Always maintain top 5 Scores and ignore the other values
		var diffs = highScoreTable.ScoreTable.Count - Settings.HighScoreCount;
		if (diffs > 0)
			highScoreTable.ScoreTable = highScoreTable.ScoreTable.GetRange(diffs, Settings.HighScoreCount);
	}

	private void DisplayHighScoreTable()
	{
		//Add the latest High Score if table does not contain any score or higher than pre existing values.
		AddToTable(Score);

		//Add the highscore values from the table to text line by line
		for (int i = highScoreTable.ScoreTable.Count - 1; i >= 0; i--)
		{
			HighScoreListText.text += highScoreTable.ScoreTable[i].ToString() + "\n";
		}
	}


	private void Update()
	{		
		// Reset the game play when "Enter" Key pressed
		if (IsGameOver && Input.GetKeyDown(KeyCode.Return))
		{
			GameOverPanel.gameObject.SetActive(false);
			IsGameOver = false;
			GameReset();
			StartGame();
		}
	}

	#endregion
}

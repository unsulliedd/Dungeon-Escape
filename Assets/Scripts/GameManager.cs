using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // End Game Properties
    public bool HasKeyToCastle;                         // Property to check if the player has the key to the castle
    public bool GameOver;                               // Property to check if the game is over

    [Header("Level Properties")]
    public int enemyCount;                              // Property to track the number of enemies in the level
    public int diamondCount;                            // Property to track the number of diamonds in the level
    public int collectedDiamondCount;                   // Property to track the number of diamonds collected by the player
    public int enemiesKilled;                           // Property to track the number of enemies killed in the level
    public int currentLevel;                            // Property to track the current level

    [Header("Star Properties")]
    private int[] starStatus = new int[3];              // Property to track the star status for the current level
    private string[,] prevStarTextStatus;               // Property to track the star text for the current level
    private int[] prevStarStatus;                       // Property to track the star image status for the current level

    // PlayerPrefs keys
    private const string LastPlayedLevelKey = "LastPlayedLevel";

    // References
    private Player player;                              // Reference to the player

    void Awake()
    {
        _instance = this;
        // Retrieve the player reference
        if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
            Debug.LogError("Player is NULL");
        // Retrieve the number of diamonds and enemies in the level
        diamondCount = GameObject.FindGameObjectsWithTag("Diamond").Length;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        // Retrieve the current level
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {
        SaveCurrentLevel(currentLevel);
        Time.timeScale = 1f;        // Set the time scale to 1 to ensure that the game is running at normal speed
        HasKeyToCastle = false;     // Set the key to the castle to false in the beginning of the level
        GameOver = false;           // Set the game over flag to false in the beginning of the level
    }

    void Update()
    {
        // Check if the player is dead
        if (!player.IsPlayerAlive())
            OnPlayerDeath();
    }

    private void OnPlayerDeath()
    {
        GameOver = true; // Set the game over flag to true
        UIManager.Instance.GameOver(); // Display the game over screen
        SaveCurrentLevel(currentLevel);
    }

    public void SaveCurrentLevel(int currentLevel)
    {
        PlayerPrefs.SetInt(LastPlayedLevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    // Restart the current level (button)
    public void Restart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Open the main menu (button)
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Give stars to the player based on the collected diamonds, killed enemies and passed time
    public void GiveStar()
    {
        if(diamondCount <= collectedDiamondCount)
        {
            UIManager.Instance.Stars(0);
            starStatus[0] = 1; // 1: Earned
        }
        if (enemyCount == enemiesKilled)
        {
            UIManager.Instance.Stars(1);
            starStatus[1] = 1; // 1: Earned
        }
        if (UIManager.Instance.time <= 300f)
        {
            UIManager.Instance.Stars(2);
            starStatus[2] = 1; // 1: Earned
        }
        // Save the star status for the current level
        SaveStarImgStatusForLevel(currentLevel);
        // Save the star text for the current level
        SaveStarText(currentLevel);
    }

    // Save the star status for the current level
    private void SaveStarImgStatusForLevel(int level)
    {
        LoadStarImgStatusForLevel(level);  // Load the previous star image status for the current level
        for (int i = 0; i < starStatus.Length; i++)
        {
            // Check if the star has already been earned
            if (prevStarStatus[i] != 1)
                // Update the star status
                PlayerPrefs.SetInt("StarStatus_Level_" + level + "_" + i, starStatus[i]);
        }
        PlayerPrefs.Save();
    }

    // Save the star text for the current level
    private void SaveStarText(int level)
    {
        LoadStarStatusForLevel(level);  // Load the previous star text status for the current level

        // String manipulation to get the saved data
        int slashIndex = prevStarTextStatus[level - 1, 0].IndexOf('/');
        string prevCollectedDiamon = prevStarTextStatus[level - 1, 0].Substring(0, slashIndex);
        int slashIndex2 = prevStarTextStatus[level - 1, 1].IndexOf('/');
        string prevKilledEnemy = prevStarTextStatus[level - 1, 1].Substring(0, slashIndex2);
        int slashIndex3 = prevStarTextStatus[level - 1, 2].IndexOf('/');
        string prevPassedTime = prevStarTextStatus[level - 1, 2].Substring(0, slashIndex3);
        TimeSpan.TryParseExact(prevPassedTime, "mm\\:ss", null, out TimeSpan prevTimeSpan);

        // Compare the current data with the saved data
        if (collectedDiamondCount > int.Parse(prevCollectedDiamon))
            SaveCollectedDiamonCount();
        // Compare the current data with the saved data
        if (enemiesKilled > int.Parse(prevKilledEnemy))
            SaveKilledEnemyCount();
        // Compare TimeSpan values
        if (GetTime() < prevTimeSpan || prevTimeSpan == TimeSpan.Zero)
            SaveTime();

        PlayerPrefs.Save();
    }

    // Save the collected diamond count method
    private void SaveCollectedDiamonCount()
    {
        PlayerPrefs.SetString("CollectedDiamondText_Level_" + currentLevel, collectedDiamondCount + "/" + diamondCount);
    }

    // Save the killed enemy count method
    private void SaveKilledEnemyCount()
    {
        PlayerPrefs.SetString("KilledEnemyText_Level_" + currentLevel, enemiesKilled + "/" + enemyCount);
    }

    // Save the passed time method
    private void SaveTime()
    {
        // Convert the decimal seconds to "mm:ss" format
        string formattedTime = $"{GetTime().Minutes:00}:{GetTime().Seconds:00}";
        PlayerPrefs.SetString("PassedTimeText_Level_" + currentLevel, formattedTime + "/05:00");
    }

    // Get the current game time
    private TimeSpan GetTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(UIManager.Instance.time);
        return timeSpan;
    }

    // Load the star image status for the current level
    private int[] LoadStarImgStatusForLevel(int level)
    {
        // Array to store the star status
        prevStarStatus = new int[3];
        // Retrieve the star status for the current level
        for (int i = 0; i < prevStarStatus.Length; i++)
            prevStarStatus[i] = PlayerPrefs.GetInt("StarStatus_Level_" + level + "_" + i, 0);
        return prevStarStatus;
    }

    // Load the star text for the current level
    private string[,] LoadStarStatusForLevel(int level)
    {
        // 2-dimensional array to store the star text
        prevStarTextStatus = new string[level, 3];
        // Retrieve the star text for the current level
        prevStarTextStatus[level - 1, 0] = PlayerPrefs.GetString("CollectedDiamondText_Level_" + level, "0/0 Diamond");
        prevStarTextStatus[level - 1, 1] = PlayerPrefs.GetString("KilledEnemyText_Level_" + level, "0/0 Enemies Killed");
        prevStarTextStatus[level - 1, 2] = PlayerPrefs.GetString("PassedTimeText_Level_" + level, "00:00/05:00 Finish Time");
        return prevStarTextStatus;
    }

    // Load next level (button)
    public void NextLevel()
    {
        SceneManager.LoadScene(currentLevel + 1);
    }
}
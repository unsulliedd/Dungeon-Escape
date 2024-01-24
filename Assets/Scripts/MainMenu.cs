using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Start Button")]
    [SerializeField] private TextMeshProUGUI _startButtonText;  // Reference to the start button

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuPanel;         // Reference to the main menu panel
    [SerializeField] private GameObject _optionsMenuPanel;      // Reference to the options menu panel
    [SerializeField] private GameObject _levelsPanel;           // Reference to the levels panel
    [SerializeField] private GameObject _statsPanel;            // Reference to the stats panel
    [SerializeField] private GameObject _settingsPanel;         // Reference to the settings panel
    [SerializeField] private GameObject _creditsPanel;          // Reference to the credits panel
    [SerializeField] private GameObject _adjustControlsPanel;   // Reference to the adjust controls panel

    [Header("Audio")]
    [SerializeField] private AudioClip clickSound;              // Reference to the click sound
    public AudioSource _mainMenuMusic;        // Reference to the main menu music

    [Header("Levels")]
    [SerializeField] private GameObject[] _level;               // Reference to the level GameObjects
    [SerializeField] private Button[] _levelButton;             // Reference to the level buttons

    [Header("Referances")]
    private GameObject currentPanel;                            // Reference to the current panel
    private AudioSource audioSource;                            // Reference to the AudioSource

    // Player Prefs
    private int lastPlayedLevel;

    private void Awake()
    {
        // Check if AudioSource is attached to the GameObject
        if (!TryGetComponent(out audioSource))
            Debug.Log("Audio Source is null");

        // Check if the player has played the game before
        lastPlayedLevel = PlayerPrefs.GetInt("LastPlayedLevel",1);
    }

    private void Start()
    {
        // Change the start button text based on the last played level
        if (lastPlayedLevel == 1)
            _startButtonText.text = "Start";
        else
            _startButtonText.text = "Continue";

        ShowPanel(_mainMenuPanel);      // Initialize UI panels
        ShowStarImgStatusForLevels();   // Initialize star image status for levels
        ShowStarStatusForLevels();      // Initialize star text status for levels
        _mainMenuMusic.Play();          // Play the main menu music
    }

    // Method to show a panel and hide the current panel
    private void ShowPanel(GameObject panelToShow)
    {
        // Switch between UI panels
        if (currentPanel != null)
            currentPanel.SetActive(false);  // Hide the current panel

        panelToShow.SetActive(true);        // Show the panel to show
        currentPanel = panelToShow;         // Set the current panel to the panel to show
    }

    // Method to start the game (button)
    public void StartGame()
    {
        ClickSound();
        SceneManager.LoadScene(lastPlayedLevel);
    }

    // Method to open the options menu (button)
    public void OpenMenu()
    {
        ClickSound();
        _mainMenuPanel.SetActive(false);
        _optionsMenuPanel.SetActive(true);
    }

    // Method to open the levels menu (button)
    public void OpenLevels()
    {
        ClickSound();
        _optionsMenuPanel.SetActive(false);
        _levelsPanel.SetActive(true);
        // Add button click listeners
        for (int i = 0; i < _levelButton.Length; i++)
            AddButtonClickListener(_levelButton[i], i);
    }

    // Add a click listener to level buttons
    private void AddButtonClickListener(Button button, int buttonIndex)
    {
        button.onClick.AddListener(() => StartLevel(buttonIndex));
    }

    // Method to start a level (button)
    private void StartLevel(int buttonIndex)
    {
        ClickSound();
        // Load the selected level when a button is clicked
        SceneManager.LoadScene(buttonIndex + 1);
    }

    // Method to display star image status for each level in the UI
    private void ShowStarImgStatusForLevels()
    {
        // Iterate through each level
        for (int levelIndex = 0; levelIndex < _level.Length; levelIndex++)
        {
            // Load the star image status for the current level
            int[] starStatus = LoadStarImgStatusForLevel(levelIndex + 1);

            // Iterate through each star index for the current level
            for (int starIndex = 0; starIndex < starStatus.Length; starIndex++)
            {
                // Get the Image components for star images in the current level
                Image[] levelStars = _level[levelIndex].GetComponentsInChildren<Image>();

                // Check if there are Image components for the current star index
                if (levelStars.Length > starIndex)
                {
                    // Set the color of the star image based on the loaded status
                    // If the star is earned (status = 1), set full color; otherwise, set a faded color
                    levelStars[starIndex].color = (starStatus[starIndex] == 1)
                        ? new Color32(255, 255, 255, 255)   // Full color
                        : new Color32(255, 255, 255, 30);   // Faded color
                }
            }
        }
    }

    // Method to show the star text status for levels
    private void ShowStarStatusForLevels()
    {
        // Iterate through each level
        for (int levelIndex = 0; levelIndex < _level.Length; levelIndex++)
        {
            // Load the star text status for the current level
            string[,] starTextStatus = LoadStarStatusForLevel(levelIndex + 1);

            // Iterate through each star index for the current level
            for (int starIndex = 0; starIndex < starTextStatus.GetLength(1); starIndex++)
            {
                // Get the TextMeshProUGUI components for star text in the current level
                TextMeshProUGUI[] levelStarsText = _level[levelIndex].GetComponentsInChildren<TextMeshProUGUI>();

                // Check if there are TextMeshProUGUI components for the current star index
                if (levelStarsText.Length > starIndex)
                    levelStarsText[starIndex].text = starTextStatus[levelIndex, starIndex];
            }
        }
    }

    // Method to load star image status for a specific level from PlayerPrefs
    private int[] LoadStarImgStatusForLevel(int level)
    {
        // Array to store star image status for each star (0: Not earned, 1: Earned)
        int[] starStatus = new int[3];

        // Iterate through each star
        for (int i = 0; i < starStatus.Length; i++)
            // Retrieve star image status from PlayerPrefs (default to 0 if not found)
            starStatus[i] = PlayerPrefs.GetInt("StarStatus_Level_" + level + "_" + i, 0);

        return starStatus;
    }

    // Method to load star text status for a specific level from PlayerPrefs
    private string[,] LoadStarStatusForLevel(int level)
    {
        // 2-dimensional array to store star text status for each star and metric
        string[,] starTextStatus = new string[level, 3];

        // Iterate through each level
        for (int i = 0; i < level; i++)
        {
            // Retrieve collected diamond count text from PlayerPrefs (default to "0/0" if not found)
            starTextStatus[i, 0] = PlayerPrefs.GetString("CollectedDiamondText_Level_" + (i + 1), "0/0");

            // Retrieve killed enemy count text from PlayerPrefs (default to "0/0" if not found)
            starTextStatus[i, 1] = PlayerPrefs.GetString("KilledEnemyText_Level_" + (i + 1), "0/0 ");

            // Retrieve passed time text from PlayerPrefs (default to "00:00/05:00" if not found)
            starTextStatus[i, 2] = PlayerPrefs.GetString("PassedTimeText_Level_" + (i + 1), "00:00/05:00");
        }
        return starTextStatus;
    }

    // Method to open the settings menu (button)
    public void OpenSettings()
    {
        ClickSound();
        _optionsMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void OpenAdjustControls()
    {
        ClickSound();
        _settingsPanel.SetActive(false);
        _adjustControlsPanel.SetActive(true);
    }

    // Method to open the stats menu (button)
    public void OpenStats()
    {
        ClickSound();
        _optionsMenuPanel.SetActive(false);
        _statsPanel.SetActive(true);
    }

    // Method to open the credits menu (button)
    public void OpenCredits()
    {
        ClickSound();
        _optionsMenuPanel.SetActive(false);
        _creditsPanel.SetActive(true);
    }

    // Method to navigate back through different UI panels
    public void NavigateBack()
    {
        ClickSound();  // Play click sound

        // Check which panel is currently active and navigate accordingly
        if (_optionsMenuPanel.activeSelf)
        {
            currentPanel = _optionsMenuPanel;
            ShowPanel(_mainMenuPanel);
        }
        else if (_levelsPanel.activeSelf)
        {
            currentPanel = _levelsPanel;
            ShowPanel(_optionsMenuPanel);
        }
        else if (_statsPanel.activeSelf)
        {
            currentPanel = _statsPanel;
            ShowPanel(_optionsMenuPanel);
        }
        else if (_settingsPanel.activeSelf)
        {
            currentPanel = _settingsPanel;
            ShowPanel(_optionsMenuPanel);
        }
        else if (_creditsPanel.activeSelf)
        {
            currentPanel = _creditsPanel;
            ShowPanel(_optionsMenuPanel);
        }
        else if(_adjustControlsPanel.activeSelf)
        {
            currentPanel = _adjustControlsPanel;
            ShowPanel(_settingsPanel);
        }
    }

    // Method to quit the game (button)
    public void QuitGame()
    {
        ClickSound();
        Application.Quit();
    }

    // Method to play a click sound
    public void ClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}

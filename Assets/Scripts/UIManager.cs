using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private GameObject _touchControls;             // Reference to the touch controls UI GameObject
    [SerializeField] private TextMeshProUGUI _HUDDiamondCount;      // Reference to the HUD diamond count text
    [SerializeField] private Image[] _healthBars;                   // Array of health bar images representing player health
    [SerializeField] private TextMeshProUGUI _timeText;             // Reference to the HUD time display text
    private float time;                                             // Variable to track elapsed game time

    [Header("Shop UI")]
    [SerializeField] private TextMeshProUGUI _playerDiamondCount;   // Reference to the player's diamond count in the shop UI
    [SerializeField] private Image _selectionIMG;                   // Reference to the selection indicator image in the shop UI
    [SerializeField] private Button item0btn;                       // Reference to the button for item 0 in the shop UI
    [SerializeField] private Button item1btn;                       // Reference to the button for item 1 in the shop UI
    [SerializeField] private Button item2btn;                       // Reference to the button for item 2 in the shop UI

    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenuPanel;            // Reference to the pause menu UI GameObject

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverPanel;             // Reference to the game over panel UI GameObject


    // Singleton pattern
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        // Activate touch controls if the device supports touch input
        if (IsTouchDevice())
            _touchControls.SetActive(true);
    }

    void Update()
    {
        // Calculate and update the game time
        CalculateTime();
    }

    // Update the displayed diamond count in the UI
    public void UpdatePlayerDiamondCount(int diamonds)
    {
        _playerDiamondCount.text = "" + diamonds + " Diamond";
        _HUDDiamondCount.text = "" + diamonds;
    }

    // Calculate and update the game time in the UI
    public void CalculateTime()
    {
        // Check if the game is currently running and if the game is not in a "Game Over" state.
        // This ensures that the time is only updated when the game is actively playing.
        if (Time.timeScale != 0 && !GameManager.Instance.GameOver)
        {
            // Increment the 'time' variable by the elapsed time since the last frame using Time.deltaTime.
            time += Time.deltaTime;

            // Convert the total time elapsed (in seconds) into minutes and seconds for display purposes.
            int minutes = Mathf.FloorToInt(time / 60F); // Calculate minutes by dividing total seconds by 60 and rounding down.
            int seconds = Mathf.FloorToInt(time - minutes * 60); // Calculate remaining seconds after subtracting minutes.

            // Update the text component (_timeText) with the formatted time string (MM:SS).
            _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Update the visual selection in the shop UI
    public void UpdateShopSelection(int yPos)
    {
        // Set the anchored position of the RectTransform associated with _selectionIMG.

        // Using Vector2 to create a new position with the same x-coordinate as the current position
        // (_selectionIMG.rectTransform.anchoredPosition.x), but with a new y-coordinate specified by the variable 'yPos'.

        // This effectively moves the UI element vertically while keeping its horizontal position unchanged.
        _selectionIMG.rectTransform.anchoredPosition = new Vector2(_selectionIMG.rectTransform.anchoredPosition.x, yPos);
    }

    // Update the availability of items in the shop UI
    public void UpdateOwnedItem(int item)
    {
        switch (item)
        {
            case 0:
                item0btn.interactable = false;
                break;
            case 1:
                item1btn.interactable = false;
                break;
            case 2:
                item2btn.interactable = false;
                break;
            case 3:
                item0btn.interactable = true;
                break;
            case 4:
                item1btn.interactable = true;
                break;
            default:
                Debug.Log("Invalid Item");
                break;
        }
    }

    // Update the displayed health bars in the UI
    public void UpdateHealth(int health)
    {
        // Check if the health is zero, indicating that the player has no remaining health.
        if (health == 0)
        {
            // Disable all health bars by iterating through each health bar in the _healthBars array.
            // This is done to visually represent that the player has no health remaining.
            foreach (var healthBar in _healthBars)
                healthBar.enabled = false;
        }

        // Iterate through each health bar in the _healthBars array.
        for (int i = 0; i < _healthBars.Length; i++)
        {
            // Check if the current iteration index (i) is less than the current health value.
            // This condition is used to determine which health bars should be enabled (visible) based on the player's health.
            if (i < health)
                // Enable the health bar at index i, indicating that the player has health remaining at this position.
                _healthBars[i].enabled = true;
            else
                // Disable the health bar at index i, indicating that the player has lost health beyond this point.
                _healthBars[i].enabled = false;
        }
    }

    // Pause the game and show the pause menu
    public void PauseGame()
    {
        _pauseMenuPanel.SetActive(true);
        Time.timeScale = 0;
        AudioManager.Instance.TogglePause();
    }

    // Resume the game from the pause menu
    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        AudioManager.Instance.PlayClickSound();
        Time.timeScale = 1;
        AudioManager.Instance.TogglePause();
    }

    // Show the game over panel after a delay
    public void GameOver()
    {
        StartCoroutine(ShowGameOverPanelAfterDelay(1f));
    }

    IEnumerator ShowGameOverPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _gameOverPanel.SetActive(true);
    }

    // Check if the device supports touch input
    bool IsTouchDevice()
    {
        return Input.touchSupported;
    }
}
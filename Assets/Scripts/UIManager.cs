using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private GameObject _touchControls;
    [SerializeField] private TextMeshProUGUI _HUDDiamondCount;
    [SerializeField] private Image[] _healthBars;

    [Header("Shop UI")]
    [SerializeField] private TextMeshProUGUI _playerDiamondCount;
    [SerializeField] private Image _selectionIMG;
    [SerializeField] private Button item0btn;
    [SerializeField] private Button item1btn;
    [SerializeField] private Button item2btn;

    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenuPanel;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverPanel;

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UIManager is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if (IsTouchDevice())
            _touchControls.SetActive(true);
    }

    public void UpdatePlayerDiamondCount(int diamonds)
    {
        _playerDiamondCount.text = "" + diamonds + " Diamond";
        _HUDDiamondCount.text = "" + diamonds;
    }

    public void UpdateShopSelection(int yPos)
    {
        _selectionIMG.rectTransform.anchoredPosition = new Vector2(_selectionIMG.rectTransform.anchoredPosition.x, yPos);
    }

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
            default:
            Debug.Log("Invalid Item");
                break;
        }
    }

    public void UpdateHealth(int health)
    {
        if (health == 0)
        {
            _healthBars[0].enabled = false;
            _healthBars[1].enabled = false;
            _healthBars[2].enabled = false;
            _healthBars[3].enabled = false;
        }

        for (int i = 0; i <= health; i++)
        {
            if (i == health)
                _healthBars[i].enabled = false;
        }
    }

    public void PauseGame()
    {
        _pauseMenuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverPanelAfterDelay(1f));
    }

    IEnumerator ShowGameOverPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _gameOverPanel.SetActive(true);
    }

    bool IsTouchDevice()
    {
        return Input.touchSupported;
    }
}

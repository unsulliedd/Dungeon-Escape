using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public bool HasKeyToCastle { get; set; }
    public bool GameOver;
    private Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        HasKeyToCastle = false;
        GameOver = false;
    }

    private void Update()
    {
        if (!player.IsPlayerAlive())
            OnPlayerDeath();
    }

    private void OnPlayerDeath()
    {
        GameOver = true;
        UIManager.Instance.GameOver();
    }

    public void Restart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

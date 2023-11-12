using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public bool HasKeyToCastle { get; set; }
    private Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _instance = this;
    }

    private void Start()
    {
        HasKeyToCastle = false;
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (!player.IsPlayerAlive())
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        UIManager.Instance.GameOver();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

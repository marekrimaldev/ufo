using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _gameFinishScreen;
    private ScoreTracker _scoreTracker;
    private TimeTracker _timeTracker;
    private bool _gameOver = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _scoreTracker = GetComponent<ScoreTracker>();
        _timeTracker = GetComponent<TimeTracker>();
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void StartGame()
    {
        _gameOverScreen.SetActive(false);
        _gameFinishScreen.SetActive(false);
        _scoreTracker.Reset();
        _timeTracker.Reset();
        _timeTracker.StartCountdown();
        _gameOver = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        StartGame();
    }

    public void OnGameFinish()
    {
        if (_gameOver)
            return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        _gameOver = true;

        //_gameFinishScreen.SetActive(true);
        Invoke("DisplayFinalScore", .1f);
    }

    private void DisplayFinalScore()
    {
        _scoreTracker.DisplayTotalScore();
    }

    public void OnGameOver()
    {
        if (_gameOver)
            return;

        _timeTracker.StopCountdown();
        _gameOver = true;

        _gameOverScreen.SetActive(true);
    }
}

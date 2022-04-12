using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text _inGameText;
    [SerializeField] private TMP_Text _gameFinishText;
    private int _score = 0;

    private void Start()
    {
        Reset();
        DisplayScore();
    }

    public void IncrementScore(int inc)
    {
        _score += inc;
        DisplayScore();
    }

    public void Reset()
    {
        _score = 0;
    }

    private void DisplayScore()
    {
        _inGameText.text = _score.ToString("D3");
    }

    public void DisplayTotalScore()
    {
        FindObjectOfType<Outro>().DisplayScore(_score);
    }
}

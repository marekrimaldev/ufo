using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Outro : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void DisplayScore(int score)
    {
        _text.text = "score: " + score.ToString("D3");
    }
}

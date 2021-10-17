using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().Play("Intro");
    }

    public void EndIntro()
    {
        GetComponent<Animator>().Play("IntroEnd");
        Invoke("LoadGameScene", 1f);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

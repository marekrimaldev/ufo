using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTimesUp;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _timeLimit = 60;
    private int _time;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _time = _timeLimit;
    }

    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    private void DisplayTime()
    {
        _text.text = _time.ToString("D2");
    }

    private IEnumerator Countdown()
    {
        WaitForSeconds ws = new WaitForSeconds(1f);
        while (_time > 0)
        {
            _time--;
            DisplayTime();
            yield return ws;
        }

        OnTimesUp.Invoke();
    }
}

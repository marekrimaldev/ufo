using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    [SerializeField] private UnityEvent OnDialogEnd;

    [SerializeField] private float _msBetweenLetters = 0.05f;
    [SerializeField] private AnimatedText[] _texts;
    private int _currText = 0;

    private void Start()
    {
        SubscribeToTexts();
        AnimateNextText();
    }

    private void SubscribeToTexts()
    {
        for (int i = 0; i < _texts.Length; i++)
        {
            _texts[i].OnTextComplete += AnimateNextText;
        }
    }

    private void AnimateNextText()
    {
        Invoke("StartAnimation", 1f);
    }

    private void StartAnimation()
    {
        if (_currText >= _texts.Length)
        {
            OnDialogEnd.Invoke();
            return;
        }

        _texts[_currText++].Animate(_msBetweenLetters);
    }


}

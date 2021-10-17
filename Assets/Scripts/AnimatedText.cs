using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimatedText : MonoBehaviour
{
    public System.Action OnTextComplete;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _content;

    private void Start()
    {
        _text.text = "";
    }

    public void Animate(float msBetweenLetters)
    {
        StartCoroutine(Animation(msBetweenLetters));
    }

    private IEnumerator Animation(float msBetweenLetters)
    {
        WaitForSeconds ws = new WaitForSeconds(msBetweenLetters / 1000);

        int idx = 0;
        while (idx < _content.Length)
        {
            string nextChar = _content[idx++].ToString();
            if (nextChar == "x")
                nextChar = "\n";

            _text.text = _text.text + nextChar;
            yield return ws;
        }

        OnTextComplete.Invoke();
    }
}

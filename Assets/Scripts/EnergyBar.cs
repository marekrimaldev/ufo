using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _blocks;
    [SerializeField] private SpriteRenderer _bolt;

    [SerializeField] private Color _fullColor = default;
    [SerializeField] private Color _emptyColor = default;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"> Value between 0 and 100 </param>
    public void ShowEnergy(float value)
    {
        float t = value / 100.0f;
        Color col = Color.Lerp(_emptyColor, _fullColor, t);
        _bolt.color = col;

        int blocksCharged = (int)(value / 10.0f);
        int i = 0;
        for (i = 0; i < blocksCharged; i++)
        {
            _blocks[i].color = col;
        }
        for (; i < 10; i++)
        {
            _blocks[i].color = Color.white;
        }
    }
}

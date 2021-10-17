using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerFish : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().Play("BigFish");
    }
}

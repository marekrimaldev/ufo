using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerFish : MonoBehaviour
{
    [SerializeField] private AudioClip _sound;

    void Start()
    {
        GetComponent<Animator>().Play("BigFish");
        GetComponent<AudioSource>().PlayOneShot(_sound);
    }
}

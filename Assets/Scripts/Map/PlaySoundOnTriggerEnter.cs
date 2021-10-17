using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTriggerEnter : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _sound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fish fish = collision.GetComponentInParent<Fish>();
        if (fish != null)
        {
            float fishWeight = fish.GetWeight();
            float t = fishWeight / 12.0f;
            float pitch = Mathf.Lerp(1.2f, 0.8f, t) + Random.Range(-0.05f, 0.05f);
            _audioSource.pitch = pitch;
            _audioSource.PlayOneShot(_sound);
        }
    }
}

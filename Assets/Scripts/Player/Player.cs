using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private UnityEvent OnPlayerDeath;
    [SerializeField] private UnityEvent<int> OnGainScore;

    [SerializeField] private Laser _laser;

    [SerializeField] private float _speed;
    private Vector2 _velocity = Vector2.zero;

    private Rigidbody2D _rb;
    private Animator _animator;

    [SerializeField] private Transform _sprite;

    [SerializeField] private AudioClip _laserSound;
    private AudioSource _audioSource;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _velocity;

        if (_rb.velocity.x > 0)
        {
            if (!_laser.IsEnabled())
                _animator.Play("FlyingRight");
            else
                _animator.Play("Idle");

        }
        else if(_rb.velocity.x < 0)
        {
            if(!_laser.IsEnabled())
                _animator.Play("FlyingLeft");
            else
                _animator.Play("Idle");

        }
        else
        {
            _animator.Play("Idle");
        }
    }

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        _velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * _speed * Time.fixedDeltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartLaser();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopLaser();
        }
    }

    private void StartLaser()
    {
        _audioSource.clip = _laserSound;
        _audioSource.Play();
        _laser.StartPullingFishes(_rb);
    }

    private void StopLaser()
    {
        _audioSource.Stop();
        _laser.StopPullingFishes();
    }

    private void Die()
    {
        OnPlayerDeath.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fish fish = collision.GetComponentInParent<Fish>();
        if (fish != null && fish.IsCatched)
        {
            OnGainScore.Invoke(fish.GetPoints());
        }
        else if (fish != null && !fish.IsCatched)
        {
            _laser.AddEnergy(-fish.GetDamage());
        }

        Debug.Log("Collision");

        KillerFish killer = collision.GetComponentInParent<KillerFish>();
        if (killer != null)
        {
            Die();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Laser _laser;

    [SerializeField] private float _speed;
    private Vector2 _velocity = Vector2.zero;

    private Rigidbody2D _rb;
    private Animator _animator;

    [SerializeField] private Transform _sprite;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _velocity;

        //_animator.SetFloat("Speed", _rb.velocity.x);
        if (_rb.velocity.x > 0)
        {
            if (!_laser.IsEnabled())
                _animator.Play("FlyingRight");
            else
                _animator.Play("Idle");

        }
        else if(_rb.velocity.x < 0)
        {
            if(_laser.IsEnabled())
                _animator.Play("FlyingLeft");
            else
                _animator.Play("Idle");

        }
        else
        {
            Debug.Log("JEBARNA");
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
        _laser.StartPullingFishes(_rb);
    }

    private void StopLaser()
    {
        _laser.StopPullingFishes();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fish fish = collision.GetComponentInParent<Fish>();
        if (fish != null && fish.IsCatched)
        {
            Debug.Log("Fish Catched");
        }
        else if (fish != null && !fish.IsCatched)
        {
            if (fish.IsDeadly())
            {
                Debug.Log("Game over");
            }
            else
            {
                Debug.Log("Losing energy");
            }
        }
    }
}

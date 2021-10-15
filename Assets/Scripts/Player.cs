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

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _velocity;
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
        _laser.gameObject.SetActive(true);
        _laser.StartPullingFishes(_rb);
    }

    private void StopLaser()
    {
        _laser.gameObject.SetActive(false);
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
            Debug.Log("Losing Life");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public System.Action<Fish> OnFishDeath;

    public float Points { get; private set; } = 1;
    public float Weight { get; private set; } = 1;

    public bool IsCatched { get; set; } = false;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void AddVelocity(Vector2 velocity)
    {
        _rb.velocity += velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COllision " + collision.name);

        if (collision.name == "Laser")
            return;

        OnFishDeath?.Invoke(this);
        Destroy(gameObject, 0.1f);
    }
}

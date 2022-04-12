using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public System.Action<Fish> OnFishDeath;

    //[SerializeField] private bool _isDeadly = false;

    [SerializeField] private float _weight = 1;
    [SerializeField] private int _points = 1;
    [SerializeField] private float _damage = 1;

    public bool IsCatched { get; set; } = false;

    [SerializeField] private bool _fakinmecoun = false;

    private Rigidbody2D _rb;

    [SerializeField] private Transform _sprite;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CheckSpriteOrientation();
    }

    private void Update()
    {
        transform.right = _rb.velocity.normalized;
    }

    private void CheckSpriteOrientation()
    {
        if (_rb.velocity.x < 0)
        {
            _sprite.Rotate(0, 180, 0);
            _sprite.Rotate(transform.right, 180);
            _sprite.Rotate(0, 180, 0);
            _sprite.Rotate(0, 0, -60);

            if (_fakinmecoun)
            {
                _sprite.Rotate(0, 0, -50);
            }
        }
    }

    public void AddVelocity(Vector2 velocity)
    {
        _rb.velocity += velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _rb.velocity = velocity;
    }

    //public bool IsDeadly()
    //{
    //    return _isDeadly;
    //}

    public float GetWeight()
    {
        return _weight;
    }

    public int GetPoints()
    {
        return _points;
    }

    public float GetDamage()
    {
        return _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Laser")
            return;

        OnFishDeath?.Invoke(this);
        Destroy(gameObject, 0.05f);
    }
}

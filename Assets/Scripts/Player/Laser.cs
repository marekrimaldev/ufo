using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _dischargeDuration = 5;
    [SerializeField] private float _chargeDuration = 5;
    private float _remainingEnergy = 100;

    [SerializeField] private float _minStrenght = 1;
    [SerializeField] private float _maxStrengt = 10;
    [SerializeField] private float _minDistance = 1;
    [SerializeField] private float _maxDistance = 10;

    [SerializeField] private UnityEvent<float> OnEnergyChange;

    private List<Fish> _fishes = new List<Fish>();

    private Rigidbody2D _pullerRb;
    private Transform _pullerDestination;
    private bool _isPulling = false;

    private Collider2D _collider;
    private SpriteRenderer _sr;

    private bool _isCharging = false;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();

        Enable(false);
    }

    public void StartPullingFishes(Rigidbody2D puller)
    {
        Enable(true);

        _pullerRb = puller;
        _pullerDestination = puller.transform;
        _isPulling = true;

        StartCoroutine(DischargeEnergy());
    }

    public void StopPullingFishes()
    {
        Enable(false);

        _pullerRb = null;
        _pullerDestination = null;
        _isPulling = false;

        if(!_isCharging)
            StartCoroutine(ChargeEnergy());
    }

    public void AddEnergy(float amount)
    {
        _remainingEnergy += amount;

        if(amount < 0)
        {
            _remainingEnergy = Mathf.Max(_remainingEnergy, 0);

            if (!_isPulling && !_isCharging)
                StartCoroutine(ChargeEnergy());
        }
        else
        {
            _remainingEnergy = Mathf.Min(_remainingEnergy, 100);
        }

        OnEnergyChange.Invoke(_remainingEnergy);
    }

    private IEnumerator DischargeEnergy()
    {
        while (_isPulling)
        {
            AddEnergy(-Time.deltaTime * 100 / _dischargeDuration);

            if(_remainingEnergy <= 0)
                StopPullingFishes();

            yield return null;
        }
    }

    private IEnumerator ChargeEnergy()
    {
        _isCharging = true;

        while (!_isPulling)
        {
            AddEnergy(Time.deltaTime * 100 / _dischargeDuration);
            yield return null;
        }

        _isCharging = false;
    }

    private void Update()
    {
        if (_isPulling)
            PullFishes();
    }

    private void PullFishes()
    {
        foreach (Fish fish in _fishes)
        {
            if (!fish)
                continue;

            float pullSpeed = CalculatePullSpeed(fish);
            Vector2 dir = (_pullerDestination.position - fish.transform.position).normalized;
            Vector2 pullerVelocityNotDown = new Vector2(_pullerRb.velocity.x, Mathf.Max(_pullerRb.velocity.y, 0));
            Vector2 velocity = pullerVelocityNotDown + (dir * (pullSpeed / fish.GetWeight()) * Time.deltaTime);
            fish.SetVelocity(velocity);
        }
    }

    private float CalculatePullSpeed(Fish fish)
    {
        if (!fish)
            return 0;

        float dist = Vector2.Distance(fish.transform.position, _pullerDestination.position);
        dist = Mathf.Clamp(dist, _minDistance, _maxDistance);
        float range = _maxDistance - _minDistance;
        float speed = Mathf.Lerp(_maxStrengt, _minStrenght, dist / range);
        return speed;
    }

    private void AddFish(Fish fish)
    {
        _fishes.Add(fish);
        fish.OnFishDeath += RemoveFish;
    }

    private void RemoveFish(Fish fish)
    {
        _fishes.Remove(fish);
        fish.OnFishDeath -= RemoveFish;
    }

    public bool IsEnabled()
    {
        return _collider.enabled;
    }

    private void Enable(bool value)
    {
        _collider.enabled = value;
        _sr.enabled = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fish fish = collision.GetComponentInParent<Fish>();
        if (fish != null)
        {
            fish.IsCatched = true;
            AddFish(fish);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Fish fish = collision.GetComponentInParent<Fish>();
        if (fish != null)
        {
            Debug.Log("Fish Removed");
            fish.SetVelocity(Vector2.zero);
            fish.IsCatched = false;
            RemoveFish(fish);
        }
    }
}

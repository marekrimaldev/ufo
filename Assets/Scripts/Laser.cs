using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _minStrenght = 1;
    [SerializeField] private float _maxStrengt = 10;
    [SerializeField] private float _minDistance = 1;
    [SerializeField] private float _maxDistance = 10;

    private List<Fish> _fishes = new List<Fish>();

    private Rigidbody2D _pullerRb;
    private Transform _pullerDestination;
    private bool _isPulling = false;

    public void StartPullingFishes(Rigidbody2D puller)
    {
        _pullerRb = puller;
        _pullerDestination = puller.transform;
        _isPulling = true;
    }

    public void StopPullingFishes()
    {
        _pullerRb = null;
        _pullerDestination = null;
        _isPulling = false;
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
            float pullSpeed = CalculatePullSpeed(fish);
            Vector2 dir = (_pullerDestination.position - fish.transform.position).normalized;
            Vector2 pullerVelocityNotDown = new Vector2(_pullerRb.velocity.x, Mathf.Max(_pullerRb.velocity.y, 0));
            Vector2 velocity = pullerVelocityNotDown + (dir * pullSpeed * Time.deltaTime);
            fish.SetVelocity(velocity);
        }
    }

    private float CalculatePullSpeed(Fish fish)
    {
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
        Debug.Log("Removing fish");

        _fishes.Remove(fish);
        fish.OnFishDeath -= RemoveFish;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fish fish = collision.GetComponentInParent<Fish>();
        if (fish != null)
        {
            Debug.Log("Fish Added");
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

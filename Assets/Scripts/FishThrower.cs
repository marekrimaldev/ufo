using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishThrower : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Fish[] _fishes;
    [SerializeField] private float _msBetweenFishes = 100;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds ws = new WaitForSeconds(_msBetweenFishes / 1000);

        while (true)
        {
            int idx = Random.Range(0, _fishes.Length);
            Fish fish = Instantiate(_fishes[idx], _spawnPoint.position, _spawnPoint.rotation);

            float t = Random.Range(1.35f * Mathf.PI, 1.65f * Mathf.PI);
            Vector3 pointOnCircle = new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0);
            Vector3 jumpDirection = (pointOnCircle).normalized;
            fish.AddVelocity(jumpDirection * 5);

            yield return ws;
        }
    }
}

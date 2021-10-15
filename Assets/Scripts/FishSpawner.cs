using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private Fish _fishPrefab;
    [SerializeField] private float _jumpForce = 50;
    [SerializeField] private Transform[] _boundaries;   // From where to where to spawn

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            SpawnFish();
            yield return new WaitForSeconds(1);
        }
    }

    private void SpawnFish()
    {
        float random01 = Random.Range(0.0f, 1.0f);
        Vector3 spawnPosition = Vector3.Lerp(_boundaries[0].position, _boundaries[1].position, random01);
        Quaternion spawnRotation = Random.rotationUniform;
        Vector3 spawnDirection = spawnRotation.eulerAngles.normalized;

        Fish fish = Instantiate(_fishPrefab, spawnPosition, spawnRotation, null);
        fish.transform.localEulerAngles = Vector3.zero;
        fish.AddVelocity(spawnDirection * _jumpForce);
    }
}

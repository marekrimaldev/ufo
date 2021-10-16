using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnableFish
{
    public Fish _fishPrefab;
    public int _rarity;
}

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _rarityMultiplier = 1.5f;
    private int _maxRarity = 0;

    [SerializeField] SpawnableFish[] _spawnableFishes;
    [SerializeField] GameObject _killerFish;
    [SerializeField] private float _minKillerSpawnTime = 5;
    [SerializeField] private float _maxKillerSpawnTime = 10;
    [SerializeField] private float _killerFishJumpForce = 10;

    [SerializeField] private float _minJumpForce = 5;
    [SerializeField] private float _maxJumpForce = 15;
    [SerializeField] private float _minJumpAngle = 0.4f;   
    [SerializeField] private float _maxJumpAngle = 0.6f;   

    [SerializeField] private Transform[] _boundaries;   // From where to where to spawn

    private void Start()
    {
        GetMaxRarity();
        StartCoroutine(Spawn());
        StartCoroutine(SpawnKillerFish());
    }

    private void GetMaxRarity()
    {
        int max = 0;
        for (int i = 0; i < _spawnableFishes.Length; i++)
        {
            if (_spawnableFishes[i]._rarity > max)
                max = _spawnableFishes[i]._rarity;
        }
        _maxRarity = max;
    }

    private Vector3 GetSpawnPosition()
    {
        float random01 = Random.Range(0.0f, 1.0f);
        return Vector3.Lerp(_boundaries[0].position, _boundaries[1].position, random01);
    }

    private void SpawnFish(Fish fishPrefab)
    {
        Vector3 spawnPosition = GetSpawnPosition();

        float t = Random.Range(_minJumpAngle * Mathf.PI, _maxJumpAngle * Mathf.PI);
        Vector3 pointOnCircle = new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0);
        Vector3 jumpDirection = (pointOnCircle).normalized;

        t = Random.Range(0.0f, 1.0f);
        float jumpForce = Mathf.Lerp(_minJumpForce, _maxJumpForce, t);

        Fish fish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity, null);
        fish.AddVelocity(jumpDirection * jumpForce);

        Destroy(fish, 5f);
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds ws = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            bool spawned = false;
            while (!spawned)
            {
                int idx = Random.Range(0, _spawnableFishes.Length);
                SpawnableFish spawnableFish = _spawnableFishes[idx];

                float weight = Random.Range(0, _maxRarity) * _rarityMultiplier;

                if (spawnableFish._rarity < weight)
                {
                    SpawnFish(spawnableFish._fishPrefab);
                    spawned = true;
                }
            }

            yield return ws;
        }
    }

    private void SpawnKiller()
    {
        Vector3 spawnPosition = GetSpawnPosition() - Vector3.up * 11;

        GameObject killer = Instantiate(_killerFish, spawnPosition, Quaternion.identity, null);
        killer.GetComponent<Rigidbody2D>().AddForce(Vector3.up * _killerFishJumpForce);

        Destroy(killer, 5f);
    }

    private IEnumerator SpawnKillerFish()
    {
        yield return new WaitForSeconds(_maxKillerSpawnTime);

        while (true)
        {
            SpawnKiller();

            float ws = Random.Range(_minKillerSpawnTime, _maxKillerSpawnTime);
            yield return new WaitForSeconds(ws);
        }
    }
}

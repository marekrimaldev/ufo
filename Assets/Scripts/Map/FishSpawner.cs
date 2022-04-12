using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnableFish
{
    public Fish FishPrefab;
    public int Rarity;
    public float JumpForce;
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

    [SerializeField] private float _minJumpAngle = 0.4f;   
    [SerializeField] private float _maxJumpAngle = 0.6f;   

    [SerializeField] private Transform[] _boundaries;   // From where to where to spawn

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _sound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        GetMaxRarity();
        StartCoroutine(Spawn());
        StartCoroutine(SpawnKillerFish());
    }

    private void GetMaxRarity()
    {
        int max = 0;
        for (int i = 0; i < _spawnableFishes.Length; i++)
        {
            if (_spawnableFishes[i].Rarity > max)
                max = _spawnableFishes[i].Rarity;
        }
        _maxRarity = max;
    }

    private Vector3 GetSpawnPosition()
    {
        float random01 = Random.Range(0.0f, 1.0f);
        return Vector3.Lerp(_boundaries[0].position, _boundaries[1].position, random01);
    }

    private void SpawnFish(SpawnableFish spawnableFish)
    {
        Vector3 spawnPosition = GetSpawnPosition();

        float t = Random.Range(_minJumpAngle * Mathf.PI, _maxJumpAngle * Mathf.PI);
        Vector3 pointOnCircle = new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0);
        Vector3 jumpDirection = (pointOnCircle).normalized;

        t = Random.Range(0.8f, 1.0f);
        float jumpForce = spawnableFish.JumpForce;

        Fish fish = Instantiate(spawnableFish.FishPrefab, spawnPosition, Quaternion.identity, null);
        fish.AddVelocity(jumpDirection * jumpForce);

        float pitch = Mathf.Lerp(1.2f, 0.8f, fish.GetWeight() / 12.0f) + Random.Range(-0.05f, 0.05f);
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(_sound);

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
                //int idx = 3;
                SpawnableFish spawnableFish = _spawnableFishes[idx];

                float weight = Random.Range(0, _maxRarity) * _rarityMultiplier;

                if (spawnableFish.Rarity < weight)
                {
                    SpawnFish(spawnableFish);
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

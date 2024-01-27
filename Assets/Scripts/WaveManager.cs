using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] private List<Transform> _waveSpawnerList;

    private int _waveNumber = 1;

    private List<GameObject> _unitToSpawn;

    [SerializeField] private GameObject _targetPrefab;

    private float _spawnCooldown;

    private float _waveCooldown;

    [SerializeField] private Transform _roamingScene;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _unitToSpawn = new();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsRoaming())
            return;

        if (_unitToSpawn.Count > 0)
        {
            _spawnCooldown -= Time.deltaTime;

            if (_spawnCooldown <= 0)
            {
                SpawnUnit(_unitToSpawn[0]);
                _spawnCooldown = 0.8f;
            }
        }

        _waveCooldown -= Time.deltaTime;
        if (_waveCooldown <= 0f)
        {
            // La wave se start avec un cooldown mais à terme faudra détecter quand la wave d'avant est finie et commencer à ce moment là
            StartNewWave();
            _waveCooldown = 30f;
        }
    }

    private void StartNewWave()
    {
        int unitNumber = Random.Range(3, 6) * _waveNumber;

        for (int i = 0; i < unitNumber; i++)
        {
            _unitToSpawn.Add(_targetPrefab);
        }

        _waveNumber++;
    }

    private void SpawnUnit(GameObject unit)
    {
        Transform spawnPoint = _waveSpawnerList[Random.Range(0, _waveSpawnerList.Count)];

        Instantiate(unit, spawnPoint.position, Quaternion.identity, _roamingScene);

        _unitToSpawn.Remove(unit);
    }
}

using System;
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

    private bool _isWaveInProgress;

    [SerializeField] private List<GameObject> _unitInGame;

    public event EventHandler OnWaveFinished;

    private float _waitTimeBetweenWaves = 4f;


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
        _unitInGame = new();
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

        if (!_isWaveInProgress)
        {
            _waveCooldown -= Time.deltaTime;
        }
        if (_waveCooldown <= 0f)
        {
            // La wave se start avec un cooldown mais à terme faudra détecter quand la wave d'avant est finie et commencer à ce moment là
            StartNewWave();
            _waveCooldown = _waitTimeBetweenWaves;
        }
    }

    private void StartNewWave()
    {
        int unitNumber = UnityEngine.Random.Range(3, 6) * _waveNumber;

        for (int i = 0; i < unitNumber; i++)
        {
            _unitToSpawn.Add(_targetPrefab);
        }

        SoundManager.Instance.PlayStartWave();
        _isWaveInProgress = true;

        _waveNumber++;
    }

    private void SpawnUnit(GameObject unit)
    {
        Transform spawnPoint = _waveSpawnerList[UnityEngine.Random.Range(0, _waveSpawnerList.Count)];

        _unitInGame.Add(Instantiate(unit, spawnPoint.position, Quaternion.identity, _roamingScene));

        _unitToSpawn.Remove(unit);
    }

    public void RemoveUnitInGame(GameObject unit)
    {
        _unitInGame.Remove(unit);

        if (_unitInGame.Count <= 0)
        {
            _isWaveInProgress = false;
            OnWaveFinished?.Invoke(this, EventArgs.Empty);
            SoundManager.Instance.PlayWaveWin();
        }
    }
}

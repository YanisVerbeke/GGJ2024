using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] private List<Transform> _waveSpawnerList;

    private int _waveNumber = 12;
    public int WaveNumber { get { return _waveNumber; } }

    private List<GameObject> _unitToSpawn;

    [SerializeField] private GameObject _targetPrefab;

    private float _spawnCooldown;

    private float _waveCooldown;

    [SerializeField] private Transform _roamingScene;

    private bool _isWaveInProgress;

    [SerializeField] private List<GameObject> _unitInGame;

    public event EventHandler OnWaveFinished;

    private float _waitTimeBetweenWaves = 4f;

    private TargetDifficulties[] _difficulties = new TargetDifficulties[3];
    private int[][] _difficultiesRandomizer = new int[10][];
    private int[] _selectedDifficulty;


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

        _difficulties[0] = TargetDifficulties.EASY;
        _difficulties[1] = TargetDifficulties.MEDIUM;
        _difficulties[2] = TargetDifficulties.HARD;

        // Set Randomizer
        _difficultiesRandomizer[0] = new int[2] { 70, 90 };
        _difficultiesRandomizer[1] = new int[2] { 65, 75 };
        _difficultiesRandomizer[2] = new int[2] { 60, 80 };
        _difficultiesRandomizer[3] = new int[2] { 55, 80 };
        _difficultiesRandomizer[4] = new int[2] { 50, 75 };
        _difficultiesRandomizer[5] = new int[2] { 40, 65 };
        _difficultiesRandomizer[6] = new int[2] { 30, 55 };
        _difficultiesRandomizer[7] = new int[2] { 20, 50 };
        _difficultiesRandomizer[8] = new int[2] { 10, 40 };
        _difficultiesRandomizer[9] = new int[2] { 5, 35 };

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
        _selectedDifficulty = _waveNumber > _difficultiesRandomizer.Length ?
            _difficultiesRandomizer[9] :
            _difficultiesRandomizer[_waveNumber - 1];
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

    public TargetDifficulties GetRandDifficultyFromWaveNumber()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        Debug.Log(rand);
        if(rand < _selectedDifficulty[0])
        {
            return TargetDifficulties.EASY;
        } 
        else if(rand >= _selectedDifficulty[0] && rand < _selectedDifficulty[1])
        {
            return TargetDifficulties.MEDIUM;
        }
        else
        {
            return TargetDifficulties.HARD;
        }
    }
}

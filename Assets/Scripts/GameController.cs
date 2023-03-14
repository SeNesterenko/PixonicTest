using System.Collections.Generic;
using Cinemachine;
using Events;
using Models;
using Plugins.SimpleEventBus.Disposables;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private ChunkSpawner _chunkSpawner;
    [SerializeField] private NearestPlanetsController _nearestPlanetsController;
    [SerializeField] private NearestChunkController _nearestChunkController;
    
    [SerializeField] private Player _player;
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    
    [SerializeField] private int _startQuantityChunks = 100;
    [SerializeField] private int _chunkSize = 100;
    [SerializeField] private int _countPlanetsInSpecialMode = 20;
    [SerializeField] private int _sizeRelativeCamera = 15;
    
    private readonly Dictionary<Vector2, ChunkModel> _chunks = new ();
    private readonly Vector3 _defaultPlanetSize = new (1, 1, 1);
    
    private Vector2 _currentIndexPosition;
    private Vector3 _currentPlayerPosition;

    private bool _isSpecialModeOn;
    private CompositeDisposable _subscriptions;

    private void Start()
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<SpecialModeActivatedEvent>(OnActiveSpecialMode),
            EventStreams.Game.Subscribe<NormalModeActivatedEvent>(OnActiveNormalMode)
        };

        InitializeControllers();
    }

    private void InitializeControllers()
    {
        var currentIndexPosition = SetCurrentPositionIndexes();
        var chunkStartIndex = (int) Mathf.Sqrt(_startQuantityChunks) / 2;

        var chunkIndexes = CollectStartIndexesChunks(chunkStartIndex, currentIndexPosition);
        var chunkModels = _chunkSpawner.Initialize(_chunkSize, chunkIndexes);

        for (var i = 0; i < chunkModels.Count; i++)
        {
            _chunks.Add(chunkIndexes[i], chunkModels[i]);
        }

        _nearestPlanetsController.Initialize(_chunks);

        _nearestChunkController.DisplayNearestChunks(currentIndexPosition, _chunks, _defaultPlanetSize);
    }

    private static List<Vector2> CollectStartIndexesChunks(int chunkStartIndex, Vector2 currentIndexPosition)
    {
        var chunkIndexes = new List<Vector2>();

        for (var x = -chunkStartIndex; x <= chunkStartIndex; x++)
        {
            for (var y = -chunkStartIndex; y <= chunkStartIndex; y++)
            {
                chunkIndexes.Add(new Vector2(currentIndexPosition.x + x, currentIndexPosition.y + y));
            }
        }

        return chunkIndexes;
    }

    private void OnActiveNormalMode(NormalModeActivatedEvent eventData)
    {
        _isSpecialModeOn = false;
        var newIndexPosition = SetCurrentPositionIndexes();
        _nearestChunkController.DisplayNearestChunks(newIndexPosition, _chunks, _defaultPlanetSize);
    }
    
    private void OnActiveSpecialMode(SpecialModeActivatedEvent eventData)
    {
        _isSpecialModeOn = true;
        ActivateNearestPlanetsController();
    }

    private void ActivateNearestPlanetsController()
    {
        var distanceCameraValue = (int) _playerCamera.m_Lens.OrthographicSize;
        var newPlanetSize = new Vector3(distanceCameraValue, distanceCameraValue) / _sizeRelativeCamera;
        _nearestPlanetsController.DisplayPlanets(_player.transform.position, distanceCameraValue,
            _countPlanetsInSpecialMode, _player.Rank, newPlanetSize);
    }

    private void InitializeChunkSpawn(int chunkStartIndex, Vector2 currentIndexPosition)
    {
        for (var x = -chunkStartIndex; x <= chunkStartIndex; x++)
        {
            for (var y = -chunkStartIndex; y <= chunkStartIndex; y++)
            {
                var chunkIndex = new Vector2(currentIndexPosition.x + x, currentIndexPosition.y + y);

                if (!_chunks.ContainsKey(chunkIndex))
                {
                    _chunks.Add(chunkIndex, _chunkSpawner.SpawnChunk(chunkIndex));
                }
            }
        }
        
        _currentIndexPosition = currentIndexPosition;
    }

    private void Update()
    {
        var newIndexPosition = SetCurrentPositionIndexes();

        if (!_chunks.ContainsKey(newIndexPosition) ||
            _chunks.ContainsKey(newIndexPosition) && _currentIndexPosition != newIndexPosition)
        {
            InitializeChunkSpawn(1, newIndexPosition);
            if (!_isSpecialModeOn)
            {
                _nearestChunkController.DisplayNearestChunks(newIndexPosition, _chunks, _defaultPlanetSize);   
            }
        }
        
        if (_isSpecialModeOn && _currentPlayerPosition != _player.transform.position)
        {
            ActivateNearestPlanetsController();
            _currentPlayerPosition = _player.transform.position;
        }
    }

    private Vector2 SetCurrentPositionIndexes()
    {
        var playerPosition = _player.gameObject.transform.position;
        var currentXIndex = (int)playerPosition.x / _chunkSize;
        var currentYIndex = (int)playerPosition.y / _chunkSize;

        return new Vector2(currentXIndex, currentYIndex);
    }

    private void OnDestroy()
    {
        _subscriptions?.Dispose();
    }
}
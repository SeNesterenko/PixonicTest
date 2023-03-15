using System;
using System.Collections.Generic;
using Events;
using Models;
using Plugins.SimpleEventBus.Disposables;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int ChunkSize => _chunkSize; 
    
    [SerializeField] private int _chunkSize = 100;
    [SerializeField] private int _startQuantityChunks = 100;
    
    private CompositeDisposable _subscriptions;

    public Dictionary<Vector2, ChunkModel> Initialize(Action<SpecialModeActivatedEvent> onActiveSpecialMode, Action<NormalModeActivatedEvent> onActiveNormalMode,
        Vector2 playerIndex, ChunkSpawner chunkSpawner, NearestPlanetsController nearestPlanetsController)
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe(onActiveSpecialMode),
            EventStreams.Game.Subscribe(onActiveNormalMode)
        };
        
        var chunkStartIndex = (int) Mathf.Sqrt(_startQuantityChunks) / 2;

        var chunksDictionary = new Dictionary<Vector2, ChunkModel>();
        var chunkIndexes = CollectStartIndexesChunks(chunkStartIndex, playerIndex);
        var chunkModels = chunkSpawner.Initialize(_chunkSize, chunkIndexes);

        for (var i = 0; i < chunkModels.Count; i++)
        {
            chunksDictionary.Add(chunkIndexes[i], chunkModels[i]);
        }

        nearestPlanetsController.Initialize(chunksDictionary);

        return chunksDictionary;
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

    private void OnDestroy()
    {
        _subscriptions?.Dispose();
    }
}
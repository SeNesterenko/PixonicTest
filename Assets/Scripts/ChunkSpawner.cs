using System.Collections.Generic;
using System.Linq;
using Events;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private int _maxRank = 10000;
    [SerializeField] private int _minRank;
    [SerializeField] private int _countPlanetsPerChunk = 30;

    private int _chunkSize;

    public List<ChunkModel> Initialize(int chunkSize, List<Vector2> indexes)
    {
        _chunkSize = chunkSize;
        return indexes.Select(StartSpawnChunk).ToList();
    }

    public ChunkModel SpawnChunk(Vector2 currentPosition)
    {
        var chunkModel = StartSpawnChunk(currentPosition);
        EventStreams.Game.Publish(new ChunkGeneratedEvent(chunkModel));

        return chunkModel;
    }

    private ChunkModel StartSpawnChunk(Vector2 currentPosition)
    {
        var xPosition = (int)(currentPosition.x + 1) * _chunkSize;
        var yPosition = (int)(currentPosition.y + 1) * _chunkSize;
        
        var planets = GeneratePlanetModels(xPosition, yPosition);

        var chunkModel = new ChunkModel(planets);
        return chunkModel;
    }

    private List<PlanetModel> GeneratePlanetModels(int xPosition, int yPosition)
    {
        var planets = new List<PlanetModel>();

        for (var i = 0; i < _countPlanetsPerChunk; i++)
        {
            var x = Random.Range(xPosition - _chunkSize, xPosition);
            var y = Random.Range(yPosition - _chunkSize, yPosition);
            var planetPosition = new Vector3(x, y, 0);

            var randomRank = Random.Range(_minRank, _maxRank);

            planets.Add(new PlanetModel(randomRank, planetPosition));
        }

        return planets;
    }
}
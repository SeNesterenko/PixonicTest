using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NearestPlanetsController : MonoBehaviour
{
    [SerializeField] private Sprite _specialPlanetSprite;
    [SerializeField] private PoolController _poolController;
    
    private PlanetSorter _planetSorter;

    public void Initialize(Dictionary<Vector2, ChunkModel> chunks)
    {
        var planets = chunks.SelectMany(chunk => chunk.Value.Planets).ToList();

        _planetSorter = new PlanetSorter(planets);
    }

    public void DisplayPlanets(Vector3 playerPosition, int fieldView, int countPlanets, int playerRank, Vector3 newSize)
    {
        var planets = _planetSorter.GetNearestPlanets(fieldView, playerPosition, countPlanets, playerRank);

        _poolController.ActivatePoolObject(planets, _specialPlanetSprite, newSize);
    }
}
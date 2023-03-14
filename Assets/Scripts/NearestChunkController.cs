using System.Collections.Generic;
using UnityEngine;

public class NearestChunkController : MonoBehaviour
{
    [SerializeField] private PoolController _poolController;
    [SerializeField] private Sprite _normalPlanetSprite;
    
    public void DisplayNearestChunks(Vector2 currentIndexPosition, Dictionary<Vector2, ChunkModel> chunks, Vector3 newSize)
    {
        var planets = new List<PlanetModel>();
        
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                var chunk = chunks[new Vector2(currentIndexPosition.x + x, currentIndexPosition.y + y)];

                planets.AddRange(chunk.Planets);
            }
        }
        
        _poolController.ActivatePoolObject(planets, _normalPlanetSprite, newSize);
    }
}
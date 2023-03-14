using System.Collections.Generic;
using UnityEngine;

public class ChunkModel
{
    public List<PlanetModel> Planets { get; }
    public Vector3 Position { get; }

    public ChunkModel(List<PlanetModel> planets, Vector3 position)
    {
        Planets = planets;
        Position = position;
    }
}
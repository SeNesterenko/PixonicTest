using UnityEngine;

public class PlanetModel
{
    public int Rank { get; }
    public Vector2 Position { get; }
    public PlanetModel(int rank, Vector2 position)
    {
        Rank = rank;
        Position = position;
    }
}
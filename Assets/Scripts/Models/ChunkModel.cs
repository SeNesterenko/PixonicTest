using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class ChunkModel
    {
        public List<PlanetModel> Planets { get; }

        public ChunkModel(List<PlanetModel> planets)
        {
            Planets = planets;
        }
    }
}
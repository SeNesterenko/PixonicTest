using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolController : MonoBehaviour
{
    [SerializeField] private PlanetView _planetView;
    
    private ObjectPool<PlanetView> _pool;
    private List<PlanetModel> _currentPlanetModels;
    private readonly List<PlanetView> _currentPlanetViews = new ();
    
    private int _index;
    private Vector3 _sizeObjects;
    private Sprite _spritePlanet;

    public void ActivatePoolObject(List<PlanetModel> planetModels, Sprite spritePlanet, Vector3 newSize)
    {
        ReloadPool();
        _sizeObjects = newSize;
        _spritePlanet = spritePlanet;
        
        _currentPlanetModels = planetModels;
        
        for (var i = 0; i < _currentPlanetModels.Count; i++)
        {
            GetPlanetView();
        }
    }
    
    private void GetPlanetView()
    {
        _currentPlanetViews.Add(_pool.Get());
    }

    private void ReloadPool()
    {
        foreach (var currentPlanetView in _currentPlanetViews)
        {
            _pool.Release(currentPlanetView);
        }

        _currentPlanetViews.Clear();

        _index = 0;
    }

    private void Awake()
    {
        _pool = new ObjectPool<PlanetView>(CreatePlanet, OnTakePlanetFromPool, OnReturnPlanetFromPoll);
    }

    private void OnReturnPlanetFromPoll(PlanetView planet)
    {
        planet.gameObject.SetActive(false);
    }

    private void OnTakePlanetFromPool(PlanetView planet)
    {
        planet.gameObject.SetActive(true);
        var planetModel = _currentPlanetModels[_index];
        planet.Initialize(planetModel, _sizeObjects, _spritePlanet);
        _index++;
    }

    private PlanetView CreatePlanet()
    {
        var planet = Instantiate(_planetView, transform);
        return planet;
    }
}
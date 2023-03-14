using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Models;
using Plugins;
using Plugins.SimpleEventBus.Disposables;
using UnityEngine;

public class PlanetSorter : IDisposable
{
    private List<PlanetModel> _sortedPlanetsByX;
    private readonly CompositeDisposable _subscriptions;
    
    public PlanetSorter(List<PlanetModel> planets)
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<ChunkGeneratedEvent>(ResortPlanets)
        };
        
        _sortedPlanetsByX = planets.OrderBy(planet => (int) planet.Position.x).ToList();
    }

    public List<PlanetModel> GetNearestPlanets(int fieldView, Vector3 currentPlayerPosition, int countPlanets, int playerRank)
    {
        var closestPlanets = new List<PlanetModel>();
        var planetsInSight = SortPlanetsInFieldOfViewByAscendingRank(fieldView, currentPlayerPosition);

        var index = -1;
        
        if (planetsInSight.Count != 0)
        {
            index = SearchNearestIndexByValue(playerRank, planetsInSight, index);
            closestPlanets = GetNearestPlanetsByRank(countPlanets, playerRank, index, planetsInSight);
        }

        return closestPlanets;
    }
    
    public void Dispose()
    {
        _subscriptions?.Dispose();
    }
    private void ResortPlanets(ChunkGeneratedEvent eventData)
    {
        var newPlanets = eventData.ChunkModel.Planets;
        
        foreach (var planet in newPlanets)
        {
            _sortedPlanetsByX.Add(planet);
        }

        _sortedPlanetsByX = _sortedPlanetsByX.OrderBy(planet => (int) planet.Position.x).ToList();
    }

    private List<PlanetModel> GetNearestPlanetsByRank(int countPlanets, int playerRank, int index, List<PlanetModel> planetsInSight)
    {
        var closestPlanets = new List<PlanetModel>();
        
        var leftBorderIndex = index - 1;
        var rightBorderIndex = index;

        while (closestPlanets.Count < countPlanets && CheckIndexNotOutOfRange(planetsInSight, leftBorderIndex, rightBorderIndex))
        {
            if (leftBorderIndex >= 0 && (rightBorderIndex >= planetsInSight.Count || CheckLeftBorderRankLessThanRightBorderRank(playerRank, planetsInSight, leftBorderIndex, rightBorderIndex)))
            {
                closestPlanets.Add(planetsInSight[leftBorderIndex]);
                leftBorderIndex--;
            }
            else
            {
                closestPlanets.Add(planetsInSight[rightBorderIndex]);
                rightBorderIndex++;
            }
        }
        
        return closestPlanets;
    }

    private static bool CheckIndexNotOutOfRange(List<PlanetModel> planetsInSight, int leftBorder, int rightBorder)
    {
        return leftBorder >= 0 || rightBorder < planetsInSight.Count;
    }

    private bool CheckLeftBorderRankLessThanRightBorderRank(int playerRank, List<PlanetModel> planetsInSight, int leftBorder, int rightBorder)
    {
        return Math.Abs(playerRank - planetsInSight[leftBorder].Rank) <=
               Math.Abs(planetsInSight[rightBorder].Rank - playerRank);
    }

    private List<PlanetModel> SortPlanetsInFieldOfViewByAscendingRank(int fieldView, Vector3 currentPlayerPosition)
    {
        var xPosition = (int) currentPlayerPosition.x;
        var yPosition = (int) currentPlayerPosition.y;

        var planetsInSight = _sortedPlanetsByX
            .TakeWhile(planet => !(planet.Position.x >= xPosition + fieldView))
            .Where(planet => planet.Position.x >= xPosition - fieldView)
            .Where(planet => planet.Position.y >= yPosition - fieldView && planet.Position.y <= yPosition + fieldView)
            .ToList();
        
        planetsInSight = planetsInSight.OrderBy(planet => planet.Rank).ToList();
        
        return planetsInSight;
    }

    private int SearchNearestIndexByValue(int playerRank, List<PlanetModel> planetsInSight, int index)
    {
        if (playerRank <= planetsInSight[0].Rank)
        {
            index = 0;
        }

        if (playerRank >= planetsInSight[^1].Rank)
        {
            index = planetsInSight.Count - 1;
        }

        if (index == -1)
        {
            index = BinarySearch.SearchIndexByKey(planetsInSight, playerRank, i => planetsInSight[i].Rank);
        }
        
        return index;
    }
}
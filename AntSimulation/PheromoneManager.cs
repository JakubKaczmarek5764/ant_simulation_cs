using System.Collections.Generic;
using System.Numerics;

namespace AntSimulation;

public class PheromoneManager : Manager
{
    public List<Pheromone> Pheromones = new List<Pheromone>();
    private static PheromoneManager _instance;
    public static PheromoneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PheromoneManager();
            }
            return _instance;
        }
    }
    public PheromoneManager()
    {
        
    }
    public (double, int, Vector2 pos) FindClosestPheromone(Vector2 pos)
    {
        double minDistance = int.MaxValue;
        int closestPheromoneIndex = -1;
        for (int i = 0; i < Pheromones.Count; i++)
        {
            Pheromone pheromone = Pheromones[i];
            double curDist = sq_distance(pheromone.Pos.X, pheromone.Pos.Y, pos.X, pos.Y);
            if (curDist < minDistance)
            {
                minDistance = curDist;
                closestPheromoneIndex = i;
            }
        }
        return (minDistance, closestPheromoneIndex, Pheromones[closestPheromoneIndex].Pos);
    }
    public void DecayPheromones()
    {
        foreach (var pheromone in Pheromones)
        {
            pheromone.Decay();
        }
    }

    public void Clear()
    {
        Pheromones.RemoveAll(x => x.LifeTime <= 0);
    }
    public bool HasDecayed(int pheromoneIndex)
    {
        return Pheromones[pheromoneIndex].LifeTime <= 0;
    }
    public bool IsEmpty()
    {
        return Pheromones.Count == 0;
    }
}
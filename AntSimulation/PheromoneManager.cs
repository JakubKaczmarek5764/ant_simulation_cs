using System.Collections.Generic;
using System.Numerics;
using System.Windows.Documents;

namespace AntSimulation;

public class PheromoneManager : Manager
{
    public List<List<Pheromone?>> PheromonesLists = new List<List<Pheromone?>>();
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
    private PheromoneManager()
    {
        for (int i = 0; i < 2; i++)
        {
            PheromonesLists.Add(new List<Pheromone?>());
        }
    }
    public void CreatePheromone(Vector2 pos, int type)
    {
        PheromonesLists[type].Add(new Pheromone(pos, type, GlobalVariables.PheromoneLifeTime));
    }
    public (double, int, Vector2 pos) FindClosestPheromone(Vector2 pos, int type)
    {
        double minDistance = int.MaxValue;
        int closestPheromoneIndex = -1;
        for (int i = 0; i < PheromonesLists[type].Count; i++)
        {
            Pheromone pheromone = PheromonesLists[type][i];
            double curDist = sq_distance(pheromone.Pos.X, pheromone.Pos.Y, pos.X, pos.Y);
            if (curDist < minDistance)
            {
                minDistance = curDist;
                closestPheromoneIndex = i;
            }
        }
        return (minDistance, closestPheromoneIndex, PheromonesLists[type][closestPheromoneIndex].Pos);
    }
    
    public void DecayPheromones()
    {
        
        foreach (var pheromoneList in PheromonesLists)
        {
            foreach (var pheromone in pheromoneList)
            {
                pheromone.Decay();
            }
        }
    }

    public void Clear()
    {
        foreach(var pheromoneList in PheromonesLists)
        {
            pheromoneList.RemoveAll(x => x.LifeTime <= 0);
        }
    }
    public bool HasDecayed(int pheromoneIndex, int type)
    {
        return PheromonesLists[type][pheromoneIndex].LifeTime <= 0;
    }
    public bool IsEmpty(int type)
    {
        return PheromonesLists[type].Count == 0;
    }
}
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace AntSimulation;

public class PheromoneManager : Manager
{
    public List<List<Pheromone?>> PheromonesLists = new List<List<Pheromone?>>();
    private List<int> PheromoneCounts = new List<int>();
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
            PheromoneCounts.Add(0);
        }
    }
    public void CreatePheromone(Vector2 pos, int type)
    {
        PheromonesLists[type].Add(new Pheromone(pos, type, GlobalVariables.PheromoneLifeTime));
        PheromoneCounts[type]++;
    }
    public (double, int, Vector2) FindClosestPheromone(Vector2 pos, Vector2 velocity, int type)
    {
        double minDistance = double.MaxValue;
        int closestPheromoneIndex = -1;
        Vector2 closestPheromonePos = Vector2.Zero; // Default value

        if (PheromonesLists[type].Count == 0) 
            return (minDistance, closestPheromoneIndex, closestPheromonePos); // No pheromones available

        Vector2 velocityDir = velocity.LengthSquared() > 0 ? Vector2.Normalize(velocity) : Vector2.UnitX; // Ensure non-zero direction

        for (int i = 0; i < PheromonesLists[type].Count; i++)
        {
            Pheromone pheromone = PheromonesLists[type][i];
            if (pheromone.LifeTime <= 0) continue; // Ignore expired pheromones

            Vector2 toPheromone = pheromone.Pos - pos;
            double curDist = Vector2.DistanceSquared(pheromone.Pos, pos);

            // Ensure the pheromone is in front of the ant using dot product
            if (Vector2.Dot(Vector2.Normalize(toPheromone), velocityDir) > 0.3f) 
            {
                if (curDist < minDistance)
                {
                    minDistance = curDist;
                    closestPheromoneIndex = i;
                    closestPheromonePos = pheromone.Pos;
                }
            }
        }

        return (minDistance, closestPheromoneIndex, closestPheromonePos);
    }
    public (double, int, Vector2) FindFurthestPheromone(Vector2 pos, Vector2 velocity, int type)
    {
        double maxDistance = double.MinValue;
        int furthestPheromoneIndex = -1;
        Vector2 furthestPheromonePos = Vector2.Zero; // Default value
        Vector2 velocityDir = velocity.LengthSquared() > 0 ? Vector2.Normalize(velocity) : Vector2.UnitX; // Ensure non-zero direction

        for (int i = 0; i < PheromonesLists[type].Count; i++)
        {
            Pheromone pheromone = PheromonesLists[type][i];
            if (pheromone.LifeTime <= 0) continue; // Ignore expired pheromones

            Vector2 toPheromone = pheromone.Pos - pos;
            double curDist = Vector2.DistanceSquared(pheromone.Pos, pos);

            // Ensure the pheromone is in front of the ant using dot product
            if (Vector2.Dot(Vector2.Normalize(toPheromone), velocityDir) > 0.3f) 
            {
                if (curDist > maxDistance)
                {
                    maxDistance = curDist;
                    furthestPheromoneIndex = i;
                    furthestPheromonePos = pheromone.Pos;
                }
            }
        }

        return (maxDistance, furthestPheromoneIndex, furthestPheromonePos);
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
    public bool IsNull(int pheromoneIndex, int type)
    {
        return pheromoneIndex + 1 > PheromonesLists[type].Count || PheromonesLists[type][pheromoneIndex] == null;
    }
    public bool HasDecayed(int pheromoneIndex, int type)
    {
        return pheromoneIndex + 1 > PheromonesLists[type].Count || PheromonesLists[type][pheromoneIndex].LifeTime <= 0;
    }
    public bool IsEmpty(int type)
    {
        return PheromoneCounts[type] == 0;
    }
}
using System.Collections.Generic;

namespace AntSimulation;

public class PheromoneCell
{
    private List<int> PheromoneIntensities = new List<int>();
    
    public PheromoneCell(int typesCount)
    {
        for (int i = 0; i < typesCount; i++)
        {
            PheromoneIntensities.Add(0);
        }
    }
    public void AddIntensity(int type, int intensity)
    {
        if (PheromoneIntensities[type] > GlobalVariables.PheromoneMaxIntensity) return;
        PheromoneIntensities[type] += intensity;
    }
    public int GetIntensity(int type)
    {
        return PheromoneIntensities[type];
    }
    public void Decay()
    {
        for (int i = 0; i < PheromoneIntensities.Count; i++)
        {
            if (PheromoneIntensities[i] > 0)
            {
                PheromoneIntensities[i]--;
            }
        }
    }
}
using System.Collections.Generic;

namespace AntSimulation;

public class Cell
{
    protected List<int> PheromoneIntensities = new List<int>();
    
    public Cell(int typesCount)
    {
        for (int i = 0; i < typesCount; i++)
        {
            PheromoneIntensities.Add(0);
        }
    }
    public void AddIntensity(int type, int intensity)
    {
        if (PheromoneIntensities[type] + intensity > GlobalVariables.PheromoneMaxIntensity) return;
        PheromoneIntensities[type] += intensity;
    }
    public int GetIntensity(int type)
    {
        return PheromoneIntensities[type];
    }
    public virtual void Decay()
    {
        for (int i = 0; i < PheromoneIntensities.Count; i++)
        {
            if (PheromoneIntensities[i] > 0)
            {
                PheromoneIntensities[i]--;
            }
        }
    }

    public virtual int GetColor()
    {
        int blueIntensity = (int) (GetIntensity( 0) * 255 / GlobalVariables.PheromoneMaxIntensity);
        int greenIntensity = (int) (GetIntensity(1) * 255 / GlobalVariables.PheromoneMaxIntensity);
        
        return (255 << 24) | (0 << 16) | (greenIntensity << 8) | blueIntensity; 
        // Format: AARRGGBB (fully opaque, red and blue components)
    }
}